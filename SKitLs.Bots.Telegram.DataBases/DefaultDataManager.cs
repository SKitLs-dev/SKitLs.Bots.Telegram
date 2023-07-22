using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.DataBases.Model;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Model.Messages;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.DataBases
{
    /// <summary>
    /// The default realization of <see cref="IDataManager"/> that realises all the basic functional.
    /// </summary>
    public class DefaultDataManager : IDataManager
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => OnReflectiveCompile;

        private IMenuManager MenuManager => Owner.ResolveService<IMenuManager>();

        public string SourceSetId => "system.source";
        public BotDataSet<IBotDataSet> SourceSet { get; set; }

        public DefaultDataManager()
        {
            var props = new DataSetProperties(SourceSetId, 0)
            {
                AllowAdd = false,
                AllowEdit = false,
                AllowExit = false,
                AllowRemove = false,
            };
            SourceSet = new BotDataSet<IBotDataSet>(SourceSetId);
        }

        public async Task Add(IBotDataSet dataSet)
        {
            dataSet.Owner = this;
            await SourceSet.AddAsync(dataSet, null);
        }
        // TODO
        public IBotDataSet GetSet(long setId) => (IBotDataSet)SourceSet.GetExisting(setId);
        public IBotDataSet GetSet(string setNameId) => SourceSet.GetAll()
            .Cast<IBotDataSet>()
            .ToList()
            .Find(x => x.DataSetId == setNameId)
            ?? throw new Exception();
        public IBotDataSet GetSet(Type setType) => SourceSet.GetAll()
            .Cast<IBotDataSet>()
            .ToList()
            .Find(x => x.DataType == setType)
            ?? throw new Exception();
        public IBotDataSet<T> GetSet<T>() where T : class, IBotDisplayable => (IBotDataSet<T>)GetSet(typeof(T));
        public List<T> GetMergedData<T>() => SourceSet.GetAll()
            .Cast<IBotDataSet>()
            .ToList()
            .FindAll(x => typeof(T).IsAssignableFrom(x.DataType))
            .SelectMany(x => x.GetAll())
            .Cast<T>()
            .ToList();

        public IBotPage GetRootPage()
        {
            var display = new DataListMessage(SourceSet);
            var dataMenu = new DataSetsMenu(this);
            var dataPage = new StaticPage("dtmngtPage", "Управление данными", display)
            {
                Menu = dataMenu,
            };
            return dataPage;
        }

        public BotArgedCallback<PaginationInfo> OpenCallback => new("dtmng", "{open set}", OpenDbPageAsync);
        private async Task OpenDbPageAsync(PaginationInfo args, SignedCallbackUpdate update)
        {
            var display = new DataListMessage(args.DataSet, args);
            var dataMenu = new DataListMenu(this, args.DataSet, args);
            var dataPage = new StaticPage("tmppage", args.DataSet.Properties.DataSetLabel, display)
            {
                Menu = dataMenu,
            };

            await MenuManager.PushPageAsync(dataPage, update);
        }

        public BotArgedCallback<ObjInfoArg> OpenObjCallback => new("dtobj", "{open object}", RaiseOpenObjectAsync);
        private async Task RaiseOpenObjectAsync(ObjInfoArg args, SignedCallbackUpdate update)
            => await args.DataSet.DisplayDataObjectAsync(args, update);
        public BotArgedCallback<PaginationInfo> AddCallback => new("addta", "➕ Добавить", RaiseAddProcessAsync);
        private async Task RaiseAddProcessAsync(PaginationInfo args, SignedCallbackUpdate update)
            => await args.DataSet.LaunchAddProcessAsync(args, update);
        public BotArgedCallback<ObjInfoArg> EditCallback => new("editdta", "✏️ Редактировать", RaiseEditProcessAsync);
        private async Task RaiseEditProcessAsync(ObjInfoArg args, SignedCallbackUpdate update)
            => await args.DataSet.LaunchEditProcessAsync(args, update);
        public BotArgedCallback<ObjInfoArg> RemoveCallback => new("deldta", "❌ Удалить", RaiseRemoveAsync);
        private async Task RaiseRemoveAsync(ObjInfoArg args, SignedCallbackUpdate update)
            => await args.DataSet.ExecuteRemoveAsync(args, update);

        private void OnReflectiveCompile(object sender, BotManager bm)
        {
            var dsRule = new ConvertRule<IBotDataSet>(dsid => ConvertResult<IBotDataSet>.OK(GetSet(dsid)));
            bm.ResolveService<IArgsSerilalizerService>().AddRule(dsRule);

            //var procRule = new ConvertRule<YesNoDataProcess<SignedCallbackUpdate>>(pid => {
            //    var lpid = long.Parse(pid);
            //    if (!processes.ContainsKey(lpid))
            //        throw new Exception();
            //    return ConvertResult<YesNoDataProcess<SignedCallbackUpdate>>.OK(processes[lpid]);
            //});
        }
        public void ApplyFor(IActionManager<SignedCallbackUpdate> callbackManager)
        {
            callbackManager.AddSafely(OpenCallback);
            callbackManager.AddSafely(AddCallback);
            callbackManager.AddSafely(OpenObjCallback);
            callbackManager.AddSafely(EditCallback);
            callbackManager.AddSafely(RemoveCallback);
        }
        public void ApplyFor(IStatefulActionManager<SignedMessageTextUpdate> entity)
        {
            foreach (var ds in SourceSet.GetAllCasted())
            {
                foreach (var proc in ds.DefinedProcesses)
                {
                    var sec = new DefaultStateSection<SignedMessageTextUpdate>();
                    sec.EnableState(proc.ProcessState);
                    sec.AddSafely((IBotAction<SignedMessageTextUpdate>)proc);
                    entity.AddSectionSafely(sec);
                }
            }
        }

        public void ApplyFor(IMenuManager entity)
        {
            entity.Define(GetRootPage());
        }
    }
}