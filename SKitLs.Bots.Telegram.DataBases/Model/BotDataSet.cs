using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses;
using SKitLs.Bots.Telegram.BotProcesses.Model.DefaultProcesses.PartialInput;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Model.Messages;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Model;

namespace SKitLs.Bots.Telegram.DataBases.Model
{
    public class BotDataSet<T> : IBotDataSet<T> where T: class, IBotDisplayable
    {
        public IDataManager? _owner;
        public IDataManager Owner
        {
            get => _owner ?? throw new Exception();
            set => _owner = _owner is null ? value : throw new Exception();
        }
        private IProcessManager ProcessManager => Owner.Owner.ResolveService<IProcessManager>();
        private IMenuManager MenuManager => Owner.Owner.ResolveService<IMenuManager>();

        public event OnDbObjUpdate<T>? ObjectAdded;
        public event OnDbObjUpdate<T>? ObjectUpdated;
        public event OnDbObjUpdate<T>? ObjectRemoved;
        public event OnDbObjUpdate<T>? NewObjectRequested;

        public Type DataType => typeof(T);
        public string DataSetId { get; private set; }
        public long SetId { get; private set; }
        public long BotArgId => SetId;

        public DataSetProperties Properties { get; set; }
        public PaginationInfo Pagination => new(this, 0, Properties.PaginationCount);

        public List<T> Data { get; private set; }
        public List<IBotProcess> DefinedProcesses { get; private set; } = new();

        public BotDataSet(string setId, IList<T>? data = null, string? dsLabel = null, Func<ICastedUpdate?, T>? createNew = null, DataSetProperties? properties = null)
        {
            DataSetId = setId;
            Data = new();
            if (data is not null)
                foreach (var item in data)
                {
                    if (Data.Find(x => x.BotArgId == item.BotArgId) is not null)
                        item.UpdateId(FirstId());
                    Data.Add(item);
                }
            CreateNew = createNew;
            Properties = properties ?? new DataSetProperties(dsLabel ?? setId);
        }

        public void UpdateId(long sid) => SetId = sid;
        public List<IBotDisplayable> GetAll() => Data.Cast<IBotDisplayable>().ToList();
        public List<T> GetAllCasted() => Data;
        public Func<ICastedUpdate?, T>? CreateNew { get; set; }
        public T GetNewMuted()
        {
            if (CreateNew is null) throw new NotImplementedException();
            var @new = CreateNew(null);
            @new.UpdateId(FirstId());
            return @new;
        }
        public async Task<T> GetNewAsync(ICastedUpdate? trigger = null)
        {
            if (CreateNew is null) throw new NotImplementedException();
            var @new = CreateNew(trigger);
            @new.UpdateId(FirstId());

            if (NewObjectRequested is not null)
                await NewObjectRequested.Invoke(@new, trigger);
            return @new;
        }
        private long FirstId()
        {
            int id = 0;
            for (int i = 0; i < Data.Count; i++)
            {
                if (id < Data[i].BotArgId)
                    return id;
                id++;
            }
            return id;
        }
        public IBotDisplayable? TryGetExisting(long bid) => Data.Find(x => x.BotArgId == bid);
        public IBotDisplayable GetExisting(long bid) => TryGetExisting(bid) ?? throw new Exception();

        public async Task AddAsync(T item, ICastedUpdate? trigger)
        {
            if (Data.Find(x => x.BotArgId == item.BotArgId) is not null)
                item.UpdateId(FirstId());
            Data.Add(item);
            if (ObjectAdded is not null)
                await ObjectAdded.Invoke(item, trigger);
        }

        public async Task RemoveAsync(T item, ICastedUpdate? trigger = null) => await RemoveAsync(item.BotArgId, trigger);
        public async Task RemoveAsync(long bid, ICastedUpdate? trigger = null)
        {
            var item = (T)GetExisting(bid);
            Data.Remove(item);
            if (ObjectRemoved is not null)
                await ObjectRemoved(item, trigger);
        }
        public async Task UpdateAsync(T item, ICastedUpdate? trigger)
        {
            var obj = (T)TryGetExisting(item.BotArgId)!;
            if (obj is not null)
                Data.Remove(obj);
            Data.Add(item);
            if (ObjectUpdated is not null)
                await ObjectUpdated(item, trigger);
        }
        public void OverrideMainPage(IOutputMessage message) => Properties.MainPageBody = message
            ?? throw new ArgumentNullException(nameof(message));

        public async Task DisplayDataObjectAsync(ObjInfoArg args, SignedCallbackUpdate update)
        {
            var obj = args.GetObject();
            var objBody = new OutputMessageText(Properties.AllowReadRows
                ? obj.FullDisplay()
                : "У вас не достаточно прав, чтобы просмотреть данные.");
            
            var objMenu = new ObjMenu(Owner, args);
            var objPage = new StaticPage("tmppage", obj.ListDisplay(), objBody)
            {
                Menu = objMenu,
            };

            await MenuManager.PushPageAsync(objPage, update);
        }

        public InputProcess<T>? AddProcess { get; private set; }
        public void UpdateAddProcess(InputProcess<T> process)
        {
            AddProcess = process;
            DefinedProcesses.Add(AddProcess);
        }
        public async Task LaunchAddProcessAsync(PaginationInfo args, SignedCallbackUpdate update)
        {
            if (AddProcess is null)
                throw new Exception();
            await ProcessManager.Run(AddProcess, new(await GetNewAsync()), update).LaunchWith(update);
        }
        public async Task AddProcessCompleted(InputProcessResult<T> args, SignedMessageTextUpdate update)
        {
            bool isOk = args.Status == OverType.Accepted && args.Result is not null;
            var result = args.Result!;
            var text = isOk
                ? $"Процесс завершён. Вот, что вы ввели:\n\n" +
                $"{result.FullDisplay()}"
                : "Процесс ввода был отменён.";
            if (isOk)
            {
                if (Properties.ConfirmAdd)
                {
                    var accept = InputDefaults.YesNoProcess<T>("yesNoProc", AddingAccepted, AddProcess.ProcessState);
                    await ProcessManager.Run(accept, InputDefaults.GetYNArgs(result), update).LaunchWith(update);
                }
                else
                {
                    await AddingAccepted(InputProcessResult<AcceptationWrapper<T>>.Accepted(new AcceptationWrapper<T>(result) { Result = true }), update);
                }
            }
            else
            {
                var mes = new OutputMessageText(text) { Menu = new MenuCleaner() };
                await update.Owner.DeliveryService.ReplyToSender(mes, update);
            }
        }
        private async Task AddingAccepted(InputProcessResult<AcceptationWrapper<T>> args, SignedMessageTextUpdate update)
        {
            bool isOk = args.Status == OverType.Accepted && args.Result is not null;
            var result = args.Result!;
            var text = isOk
                ? $"Данные сохранены."
                : "Процесс ввода был отменён.";
            if (isOk)
            {
                if (result.Result)
                {
                    await AddAsync(result.Value, update);
                }
            }
            var mes = new OutputMessageText(text) { Menu = new MenuCleaner() };
            await update.Owner.DeliveryService.ReplyToSender(mes, update);
        }

        public InputProcess<T>? EditProcess { get; private set; }
        public void UpdateEditProcess(InputProcess<T> process)
        {
            EditProcess = process;
            DefinedProcesses.Add(EditProcess);
        }
        public async Task LaunchEditProcessAsync(ObjInfoArg args, SignedCallbackUpdate update)
        {
            if (EditProcess is null)
                throw new Exception();
            await ProcessManager.Run(EditProcess, new((T)args.GetObject()), update).LaunchWith(update);
        }
        public async Task EditProcessCompleted(InputProcessResult<T> args, SignedMessageTextUpdate update)
        {
            bool isOk = args.Status == OverType.Accepted && args.Result is not null;
            var result = args.Result!;
            var text = isOk
                ? $"Процесс завершён. Вот, что вы ввели:\n\n" +
                $"{result.FullDisplay()}"
                : "Процесс ввода был отменён.";
            if (isOk)
            {
                if (Properties.ConfirmAdd)
                {
                    var accept = InputDefaults.YesNoProcess<T>("yesNoProc", EditAccepted, AddProcess.ProcessState);
                    await ProcessManager.Run(accept, InputDefaults.GetYNArgs(result), update).LaunchWith(update);
                }
                else
                {
                    await EditAccepted(InputProcessResult<AcceptationWrapper<T>>.Accepted(new AcceptationWrapper<T>(result) { Result = true }), update);
                }
            }
            else
            {
                var mes = new OutputMessageText(text) { Menu = new MenuCleaner() };
                await update.Owner.DeliveryService.ReplyToSender(mes, update);
            }
        }
        private async Task EditAccepted(InputProcessResult<AcceptationWrapper<T>> args, SignedMessageTextUpdate update)
        {
            bool isOk = args.Status == OverType.Accepted && args.Result is not null;
            var result = args.Result!;
            var text = isOk
                ? $"Данные были обновлены"
                : "Процесс ввода был отменён.";
            if (isOk)
            {
                if (result.Result)
                {
                    await UpdateAsync(result.Value, update);
                }
            }
            var mes = new OutputMessageText(text) { Menu = new MenuCleaner() };
            await update.Owner.DeliveryService.ReplyToSender(mes, update);
        }

        public InputProcess<AcceptationWrapper<T>>? RemoveProcess { get; private set; }
        public void UpdateRemoveProcess(InputProcess<AcceptationWrapper<T>> process)
        {
            RemoveProcess = process;
            DefinedProcesses.Add(RemoveProcess);
        }
        public async Task ExecuteRemoveAsync(ObjInfoArg args, SignedCallbackUpdate update)
        {
            if (RemoveProcess is null)
                throw new Exception();

            if (Properties.ConfirmRemove)
            {
                await ProcessManager.Run(RemoveProcess, new(new((T)args.GetObject())), update).LaunchWith(update);
            }
            else
                await RemoveAccepted(InputProcessResult<AcceptationWrapper<T>>.Accepted(new AcceptationWrapper<T>((T)args.GetObject()) { Result = true }), update);
        }
        public async Task RemoveAccepted(InputProcessResult<AcceptationWrapper<T>> args, SignedMessageTextUpdate update)
            => await RemoveAccepted(args, update);
        public async Task RemoveAccepted(InputProcessResult<AcceptationWrapper<T>> args, ISignedUpdate update)
        {
            bool isOk = args.Status == OverType.Accepted && args.Result is not null;
            var result = args.Result!;
            var text = isOk
                ? $"Объект был удалён"
                : "Процесс удаления был отменён.";
            if (isOk)
            {
                if (result.Result)
                {
                    await RemoveAsync(result.Value, update);
                }
            }
            var mes = new OutputMessageText(text) { Menu = new MenuCleaner() };
            await update.Owner.DeliveryService.ReplyToSender(mes, update);
        }


        public string GetPacked() => DataSetId;

        public string ListDisplay() => Properties.DataSetLabel;
        public string ListLabel() => Properties.DataSetLabel;
        public string FullDisplay(params string[] args) => Properties.DataSetLabel;

        public override string ToString() => Properties.DataSetLabel ?? DataSetId;
    }
}