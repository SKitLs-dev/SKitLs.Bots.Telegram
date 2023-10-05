using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Reply;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.BotProcesses.Model;
using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults;
using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.ComplexShot;
using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Util;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.DataBases.Extensions;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Model.Messages;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using Settings = SKitLs.Bots.Telegram.DataBases.resources.settings.SkDBSettings;

namespace SKitLs.Bots.Telegram.DataBases.Model.Datasets
{
    public abstract class ListDataSetBase<T> : IBotDataSet<T> where T : class, IBotDisplayable
    {
        public event OnDbObjUpdate<T>? ObjectAdded;
        public event OnDbObjUpdate<T>? ObjectUpdated;
        public event OnDbObjUpdate<T>? ObjectRemoved;
        public event OnDbObjUpdate<T>? NewObjectRequested;

        public BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => OnReflectiveCompile;

        protected IDataManager DataManager => Owner.ResolveService<IDataManager>();
        protected IProcessManager ProcessManager => Owner.ResolveService<IProcessManager>();
        protected IMenuManager MenuManager => Owner.ResolveService<IMenuManager>();

        public Type DataType => typeof(T);
        public string DataSetId { get; private init; }
        public long SetId { get; private set; }
        public long BotArgId => SetId;
        public virtual void UpdateId(long sid) => SetId = sid;

        public List<T> Data { get; private set; }
        public List<IBotProcess> DefinedProcesses { get; } = new();
        public DataSetProperties Properties { get; set; }
        public PaginationInfo Pagination => new(this, 0, Properties.PaginationCount);

        public ListDataSetBase(string setId, IList<T>? data = null, string? dsLabel = null, Func<ICastedUpdate?, T>? createNew = null, DataSetProperties? properties = null)
        {
            DataSetId = setId;
            CreateNew = createNew;
            Properties = properties ?? new DataSetProperties(dsLabel ?? setId);
            Data = new();
            data?.ToList().ForEach(async x => await AddAsync(x, null, true));
        }

        public int Count => Data.Count;
        public T First() => Data.First();
        public IBotDisplayable FirstDisplayable() => First();
        public List<T> GetAll() => Data;
        public List<IBotDisplayable> GetAllDisplayable() => Data.Cast<IBotDisplayable>().ToList();
        public T? Find(Predicate<T> match) => Data.Find(match);

        public abstract List<IBotDisplayable> GetUserSubsetDisplayable(long telegramId);
        public abstract List<IBotDisplayable> GetContextSubsetDisplayable(ISignedUpdate update);
        public abstract List<T> GetUserSubset(long telegramId);
        public abstract List<T> GetContextSubset(ISignedUpdate update);

        public Func<ICastedUpdate?, T>? CreateNew { get; set; }
        public async Task<T> GetNewAsync(ICastedUpdate? trigger = null, bool mute = false)
        {
            // TODO
            if (CreateNew is null) throw new NotImplementedException();
            var @new = CreateNew(trigger);
            @new.UpdateId(Data.FirstId());

            if (!mute && NewObjectRequested is not null)
                await NewObjectRequested.Invoke(@new, trigger);
            return @new;
        }
        public IBotDisplayable GetExisting(long bid) => TryGetExisting(bid) ?? throw new NotDefinedException(this, DataType, bid.ToString());
        public IBotDisplayable? TryGetExisting(long bid) => Data.Find(x => x.BotArgId == bid);

        public async Task AddAsync(T item, ICastedUpdate? trigger) => await AddAsync(item, trigger, false);
        private async Task AddAsync(T item, ICastedUpdate? trigger, bool mute)
        {
            if (Data.Find(x => x.BotArgId == item.BotArgId) is not null && Properties.EnableAutoIdUpdate)
                item.UpdateId(Data.FirstId());
            Data.Add(item);
            Data = Data.OrderBy(x => x.BotArgId).ToList();
            if (ObjectAdded is not null && !mute)
                await ObjectAdded.Invoke(item, trigger);
        }

        public async Task RemoveAsync(T item, ICastedUpdate? trigger = null) => await RemoveAsync(item.BotArgId, trigger);
        public async Task RemoveAsync(long bid, ICastedUpdate? trigger = null)
        {
            var item = (T)GetExisting(bid);
            Data.Remove(item);
            Data = Data.OrderBy(x => x.BotArgId).ToList();
            if (ObjectRemoved is not null)
                await ObjectRemoved(item, trigger);
        }
        public async Task UpdateAsync(T item, ICastedUpdate? trigger)
        {
            var obj = (T)TryGetExisting(item.BotArgId)!;
            if (obj is not null)
                Data.Remove(obj);
            Data.Add(item);
            Data = Data.OrderBy(x => x.BotArgId).ToList();
            if (ObjectUpdated is not null)
                await ObjectUpdated(item, trigger);
        }

        protected List<BotArgedCallback<DtoArg<T>>> ObjectActionsList { get; } = new();
        public void AddAction(BotArgedCallback<DtoArg<T>> action) => ObjectActionsList.Add(action);
        public List<BotArgedCallback<DtoArg<T>>> GetObjectActions() => ObjectActionsList;

        public virtual async Task DisplayObjectDataAsync(ObjInfoArg args, SignedCallbackUpdate update)
        {
            var obj = args.GetObject();
            var objBody = new OutputMessageText(Properties.AllowReadRows
                ? obj.FullDisplay()
                : Owner.ResolveBotString("display.db.ReadRowsForbiddenMes"));

            var objPage = new WidgetPage(Settings.DataTempPageId, obj.ListDisplay(), objBody, new ObjMenu<T>(DataManager, args));

            await MenuManager.PushPageAsync(objPage, update);
        }

        public void InitializeProcesses(BotManager owner)
        {
            var term = owner.ResolveBotString("system.db.TerminationalKey");

            if (Properties.AllowAdd)
            {
                AddProcess ??= new ComplexShotInputProcess<T>(
                    processData: ProcessData(DbActionType.Add),
                    startupMessage: async (a, u) => await StaticMessage(Settings.AddStartupMessageLK, FillHelper.GetShotLabels(DataType)),
                    overByCallback: When_AddProcessOver,
                    confirmMessage: async (a, u) => await StaticMessage(Settings.AddItConfirmMessageLK, a.BuildingInstance.FullDisplay()));
                DefinedProcesses.Add(AddProcess);
            }
            if (Properties.AllowEdit)
            {
                EditProcess ??= new ComplexShotInputProcess<T>(
                    processData: ProcessData(DbActionType.Edit),
                    startupMessage: async (a, u) => await StaticMessage(Settings.EditStartupMessageLK, a.BuildingInstance.FullDisplay()),
                    overByCallback: When_EditProcessOver,
                    confirmMessage: async (a, u) => await StaticMessage(Settings.AddItConfirmMessageLK, a.BuildingInstance.FullDisplay()));
                DefinedProcesses.Add(EditProcess);
            }
            if (Properties.AllowRemove)
            {
                RemoveProcess ??= new TerminatorProcess<T>(
                    processData: ProcessData(DbActionType.Remove),
                    overByCallback: When_RemoveProcessConfirmed,
                    confirmMessage: async (a, u) => await StaticMessage(Settings.RemoveConfirmMessageLK));
                DefinedProcesses.Add(RemoveProcess);
            }

            IST ProcessData(DbActionType actionType) => Settings.GetProcessData(this, actionType);
            async Task<IOutputMessage> StaticMessage(string key, params string?[] format)
                => await Task.FromResult(new OutputMessageText(owner.ResolveBotString(key, format)));
        }

        public void UpdateProcess(TextInputsProcessBase<T> process, DbActionType actionType)
        {
            var ist = Settings.GetProcessData(this, actionType);
            ist.TerminationalKey = process.TerminationalKey;
            process.UpdateIST(ist);
            process.Confirmation?.UpdateIST(ist);
            if (actionType == DbActionType.Add)
                AddProcess = process;
            else if (actionType == DbActionType.Edit)
                EditProcess = process;
            else if (actionType == DbActionType.Remove)
                RemoveProcess = process;
        }

        public TextInputsProcessBase<T>? AddProcess { get; private set; }
        public virtual async Task LaunchAddProcessAsync(PaginationInfo args, SignedCallbackUpdate update)
        {
            // TODO
            if (AddProcess is null)
            {
                throw new Exception();
            }
            else
                await ProcessManager.Run(AddProcess, new(await GetNewAsync()), update).LaunchWith(update);
        }
        public virtual async Task When_AddProcessOver(TextInputsArguments<T> args, SignedCallbackUpdate update)
        {
            var text = ResolveStatus(args.CompleteStatus, DbActionType.Add);
            if (args.CompleteStatus == ProcessCompleteStatus.Success)
            {
                text = string.Format(text, args.BuildingInstance.FullDisplay());
                await AddAsync(args.BuildingInstance, update);
            }
            var mes = await new OutputMessageText(text) { Menu = new ReplyCleaner() }.BuildContentAsync(update);
            await update.Owner.DeliveryService.AnswerSenderAsync(new EditWrapper(mes, update.TriggerMessageId), update);
        }

        public TextInputsProcessBase<T>? EditProcess { get; private set; }
        public virtual async Task LaunchEditProcessAsync(ObjInfoArg args, SignedCallbackUpdate update)
        {
            // TODO
            if (EditProcess is null)
            {
                throw new Exception();
            }
            else
                await ProcessManager.Run(EditProcess, new(args.GetObject<T>()), update).LaunchWith(update);
        }
        public virtual async Task When_EditProcessOver(TextInputsArguments<T> args, SignedCallbackUpdate update)
        {
            var text = ResolveStatus(args.CompleteStatus, DbActionType.Edit);
            if (args.CompleteStatus == ProcessCompleteStatus.Success)
            {
                text = string.Format(text, args.BuildingInstance.FullDisplay());
                await UpdateAsync(args.BuildingInstance, update);
            }
            var mes = await new OutputMessageText(text) { Menu = new ReplyCleaner() }.BuildContentAsync(update);
            await update.Owner.DeliveryService.AnswerSenderAsync(new EditWrapper(mes, update.TriggerMessageId), update);
        }

        public TextInputsProcessBase<T>? RemoveProcess { get; private set; }
        public virtual async Task LaunchRemoveProcessAsync(ObjInfoArg args, SignedCallbackUpdate update)
        {
            // TODO
            if (RemoveProcess is null)
            {
                throw new Exception();
            }
            else
                await ((TextInputsRunningBase<TerminatorProcess<T>, T>)ProcessManager.Run(RemoveProcess, new(args.GetObject<T>()), update))
                    .TerminateAsync(update);
        }
        public virtual async Task When_RemoveProcessConfirmed(TextInputsArguments<T> args, SignedCallbackUpdate update)
        {
            var text = ResolveStatus(args.CompleteStatus, DbActionType.Remove);
            if (args.CompleteStatus == ProcessCompleteStatus.Success)
            {
                await RemoveAsync(args.BuildingInstance, update);
            }
            var mes = await new OutputMessageText(text) { Menu = new ReplyCleaner() }.BuildContentAsync(update);
            await update.Owner.DeliveryService.AnswerSenderAsync(new EditWrapper(mes, update.TriggerMessageId), update);
        }

        // TODO : GetName() for PCS
        public string ResolveStatus(ProcessCompleteStatus status, DbActionType action) => Owner.ResolveBotString(status switch
        {
            ProcessCompleteStatus.Success => $"display.db.{Enum.GetName(action)}Success",
            ProcessCompleteStatus.Canceled => $"display.db.{Enum.GetName(action)}Canceled",
            _ => $"display.db.{Enum.GetName(action)}Unknown",
        });

        public virtual string ListDisplay() => Properties.DataSetLabel;
        public virtual string ListLabel() => Properties.DataSetLabel;
        public virtual string FullDisplay(params string[] args) => Properties.DataSetLabel;

        protected void OnReflectiveCompile(object sender, BotManager owner)
        {
            InitializeProcesses(owner);
            Data.Where(x => x is IOwnerCompilable)
                .Cast<IOwnerCompilable>()
                .ToList()
                .ForEach(x => x.ReflectiveCompile(sender, owner));
        }

        public virtual string GetPacked() => DataSetId;
        public override string ToString() => Properties.DataSetLabel ?? DataSetId;
    }
}