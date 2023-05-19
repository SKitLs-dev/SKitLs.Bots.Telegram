using SKitLs.Bots.Telegram.AdvancedMessages.Model;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.DataBases.external;
using SKitLs.Bots.Telegram.DataBases.Model;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Model.Messages;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace SKitLs.Bots.Telegram.DataBases
{
    public class DefaultDataManager : IDataManager
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException();
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => OnReflectiveCompile;

        private IArgsSerilalizerService Serializer => Owner.ResolveService<IArgsSerilalizerService>();
        private IMenuManager MenuManager => Owner.ResolveService<IMenuManager>();

        public string SourceSetId => "system.source";
        public IBotDataSet SourceSet => GetSet(SourceSetId);
        public List<IBotDataSet> DataSets { get; set; } = new();

        public DefaultDataManager()
        {
            DataSets.Add(new BotDataSet(SourceSetId, DataSets.Cast<IBotDisplayable>().ToList()));
        }

        public void Add(IBotDataSet dataSet)
        {
            DataSets.Add(dataSet);

            // Update Source Set
            DataSets.Remove(GetSet(SourceSetId));
            DataSets.Add(new BotDataSet(SourceSetId, DataSets.Cast<IBotDisplayable>().ToList()));
        }
        // TODO
        public IBotDataSet GetSet(string setId) => DataSets.Find(x => x.SetId == setId) ?? throw new Exception();

        public IPageWrap GetRootPage()
        {
            var display = new DataListMessage(SourceSet);
            var dataMenu = new DataSetsMenu(this);
            var dataPage = new PageWrap("dtmngtPage", "Управление данными", display)
            {
                Menu = dataMenu,
                LockId = true,
            };
            return dataPage;
        }

        public BotArgedCallback<PaginationInfo> OpenCallabck => new("dtmng", "{open set}", OpenDbPageAsync);
        private async Task OpenDbPageAsync(IBotAction<SignedCallbackUpdate> trigger, PaginationInfo args, SignedCallbackUpdate update)
        {
            var display = new DataListMessage(args.DataSet, args);
            var dataMenu = new DataListMenu(this, args.DataSet, args);
            var dataPage = new PageWrap("tmppage", args.DataSet.SetLabel, display)
            {
                Menu = dataMenu,
            };

            await MenuManager.PushMenuPageAsync(GetRootPage(), dataPage, update);
        }

        public BotArgedCallback<ObjInfoArg> OpenObjCallabck => new("dtobj", "{open object}", OpenDataObjAsync);
        private async Task OpenDataObjAsync(IArgedAction<ObjInfoArg, SignedCallbackUpdate> trigger, ObjInfoArg args, SignedCallbackUpdate update)
        {
            var obj = args.GetObject();
            var objBody = new OutputMessageText(obj.FullDisplay());
            var objMenu = new ObjMenu(this, args);
            var objPage = new PageWrap("tmppage", obj.ListDisplay(), objBody)
            {
                Menu = objMenu,
            };

            await MenuManager.PushMenuPageAsync(null, objPage, update);
        }

        public BotArgedCallback<PaginationInfo> AddCallabck => new("addta", "➕ Добавить", LaunchAddProcessAsync);
        private async Task LaunchAddProcessAsync(IArgedAction<PaginationInfo, SignedCallbackUpdate> trigger, PaginationInfo args, SignedCallbackUpdate update)
        {
            if (update.Sender is not IStatefulUser stu) throw new Exception();

            stu.State = 0;

            var proc = new YesNoDataProcess<SignedCallbackUpdate>(args.DataSet, AddYesAsync, AddNoAsync)
            {
                OnAny = ResetStateAsync
            };
            processes.Add(update.Sender.TelegramId, proc);

            await Owner.DelieveryService.ReplyToSender(new EditWrapper(
                new OutputMessageText("Введите следующие данные:"), update.TriggerMessageId), update);
        }
        private async Task AddYesAsync(IArgedAction<ConfrimArg<SignedCallbackUpdate>, SignedCallbackUpdate> trigger, ConfrimArg<SignedCallbackUpdate> args, SignedCallbackUpdate update)
        {
            args.Process.DataSet.Add();
            await ResultResponse(args, "", update);
        }
        private async Task AddNoAsync(IArgedAction<ConfrimArg<SignedCallbackUpdate>, SignedCallbackUpdate> trigger, ConfrimArg<SignedCallbackUpdate> args, SignedCallbackUpdate update)
        {
            await ResultResponse(args, "", update);
        }
        public BotArgedCallback<ObjInfoArg> EditCallabck => new("editdta", "✏️ Редактировать", LaunchEditProcessAsync);
        private async Task LaunchEditProcessAsync(IArgedAction<ObjInfoArg, SignedCallbackUpdate> trigger, ObjInfoArg args, SignedCallbackUpdate update)
        {
            if (update.Sender is not IStatefulUser stu) throw new Exception();

            stu.State = 0;

            var proc = new YesNoDataProcess<SignedCallbackUpdate>(args.DataSet, EditYesAsync, EditNoAsync)
            {
                OnAny = ResetStateAsync
            };
            processes.Add(update.Sender.TelegramId, proc);

            await Owner.DelieveryService.ReplyToSender(new EditWrapper(
                new OutputMessageText("Введите следующие данные:"), update.TriggerMessageId), update);
        }
        private async Task EditYesAsync(IArgedAction<ConfrimArg<SignedCallbackUpdate>, SignedCallbackUpdate> trigger, ConfrimArg<SignedCallbackUpdate> args, SignedCallbackUpdate update)
        {
            args.Process.DataSet.Add();
            await ResultResponse(args, "", update);
        }
        private async Task EditNoAsync(IArgedAction<ConfrimArg<SignedCallbackUpdate>, SignedCallbackUpdate> trigger, ConfrimArg<SignedCallbackUpdate> args, SignedCallbackUpdate update)
        {
            await ResultResponse(args, "", update);
        }
        public BotArgedCallback<ObjInfoArg> RemoveCallabck => new("deldta", "❌ Удалить", RemoveAsync);
        private async Task RemoveAsync(IArgedAction<ObjInfoArg, SignedCallbackUpdate> trigger, ObjInfoArg args, SignedCallbackUpdate update)
        {
            if (update.Sender is not IStatefulUser stu) throw new Exception();

            stu.State = 0;

            var proc = new YesNoDataProcess<SignedCallbackUpdate>(args.DataSet, RemoveYesAsync, RemoveNoAsync)
            {
                OnAny = ResetStateAsync
            };
            processes.Add(update.Sender.TelegramId, proc);

            await Owner.DelieveryService.ReplyToSender(new EditWrapper(
                new OutputMessageText("Введите следующие данные:"), update.TriggerMessageId), update);
        }
        private async Task RemoveYesAsync(IArgedAction<ConfrimArg<SignedCallbackUpdate>, SignedCallbackUpdate> trigger, ConfrimArg<SignedCallbackUpdate> args, SignedCallbackUpdate update)
        {
            args.Process.DataSet.Remove(args.Process.Data.BotArgId);
            await ResultResponse(args, $"Объек был успешно удалён!", update);
        }
        private async Task RemoveNoAsync(IArgedAction<ConfrimArg<SignedCallbackUpdate>, SignedCallbackUpdate> trigger, ConfrimArg<SignedCallbackUpdate> args, SignedCallbackUpdate update)
            => await ResultResponse(args, $"Удаление {args.Process.Data?.ListDisplay() ?? "???"} было отменено", update);

        // TODO
        private Task ResetStateAsync(IArgedAction<ConfrimArg<SignedCallbackUpdate>, SignedCallbackUpdate> trigger, ConfrimArg<SignedCallbackUpdate> args, SignedCallbackUpdate update)
        {
            if (!processes.ContainsKey(update.Sender.TelegramId))
                throw new Exception();
            processes.Remove(update.Sender.TelegramId);
            
            if (update.Sender is not IStatefulUser stu)
                throw new Exception();
            stu.State = 0;
            
            return Task.CompletedTask;
        }
        private async Task ResultResponse(ConfrimArg<SignedCallbackUpdate> args, string response, SignedCallbackUpdate update)
        {
            var respMes = new OutputMessageText(response);
            respMes.AddMenu(new PairedInlineMenu("<< Выйти", OpenCallabck.GetSerializedData(args.Process.DataSet.PaginationInfo, Serializer)));
            await Owner.DelieveryService.ReplyToSender(new EditWrapper(respMes, update.TriggerMessageId), update);
        }

        private Dictionary<long, YesNoDataProcess<SignedCallbackUpdate>> processes = new();
        public BotArgedCallback<ConfrimArg<SignedCallbackUpdate>> ConfirmCallabck => new("ConfirmationAlert", "Да/Нет", ConfirmAsync);
        private async Task ConfirmAsync(IArgedAction<ConfrimArg<SignedCallbackUpdate>, SignedCallbackUpdate> trigger, ConfrimArg<SignedCallbackUpdate> args, SignedCallbackUpdate update)
        {
            if (!processes.ContainsKey(update.Sender.TelegramId))
                throw new Exception();

            await processes[update.Sender.TelegramId].Excecute(trigger, args, update);
        }

        public DefaultTextInput DataInputAction => new(string.Empty, HandleDataInputAsync);
        private async Task HandleDataInputAsync(IBotAction<SignedMessageTextUpdate> trigger, SignedMessageTextUpdate update)
        {
            if (!processes.ContainsKey(update.Sender.TelegramId))
                throw new Exception();
            var proc = processes[update.Sender.TelegramId];

            // Try to unpack and parse data
            // Data counts wrong
            var res = (ConvertResult<IBotDisplayable>?)Serializer.GetType()
                .GetMethod(nameof(Serializer.Deserialize))!
                .MakeGenericMethod(proc.DataType)
                .Invoke(Serializer, new object[] { update.Text, '\n' });

            //var res = Serializer.Deserialize<IBotDisplayable>(update.Text, '\n');

            if (res is null || res.ResultType != ConvertResultType.Ok)
            {
                var mes = new MultiblockMessage();
                mes.AddBlock("Следующие данные были введены неверно:");
                mes.AddBlock(res?.Message ?? "Внутренняя ошибка преобразования");
                mes.AddBlock("Введите, пожалуйста, заново");
                await Owner.DelieveryService.ReplyToSender(mes, update);
            }
            else
            {
                var value = res.Value;
                proc.Data = value;
                var mes = new MultiblockMessage();
                mes.AddBlock("Проверьте введённые данные:");
                mes.AddBlock((value as IBotDisplayable)!.FullDisplay());
                var menu = new PairedInlineMenu()
                {
                    ColumnsCount = 2
                };
                menu.Add("Да", ConfirmCallabck.GetSerializedData(new ConfrimArg<SignedCallbackUpdate>(true, proc), Serializer));
                menu.Add("Нет", ConfirmCallabck.GetSerializedData(new ConfrimArg<SignedCallbackUpdate>(false, proc), Serializer));
                mes.Menu = menu;
                await Owner.DelieveryService.ReplyToSender(mes, update);
            }
        }

        private void OnReflectiveCompile(object sender, BotManager bm)
        {
            var dsRule = new ConvertRule<IBotDataSet>(dsid => ConvertResult<IBotDataSet>.OK(GetSet(dsid)));
            bm.ResolveService<IArgsSerilalizerService>().AddRule(dsRule);

            var procRule = new ConvertRule<YesNoDataProcess<SignedCallbackUpdate>>(pid => {
                var lpid = long.Parse(pid);
                if (!processes.ContainsKey(lpid))
                    throw new Exception();
                return ConvertResult<YesNoDataProcess<SignedCallbackUpdate>>.OK(processes[lpid]);
            });
        }
        public void ApplyFor(IActionManager<SignedCallbackUpdate> callbackManager)
        {
            callbackManager.AddSafely(OpenCallabck);
            callbackManager.AddSafely(AddCallabck);
            callbackManager.AddSafely(OpenObjCallabck);
            callbackManager.AddSafely(EditCallabck);
            callbackManager.AddSafely(RemoveCallabck);
        }
    }
}