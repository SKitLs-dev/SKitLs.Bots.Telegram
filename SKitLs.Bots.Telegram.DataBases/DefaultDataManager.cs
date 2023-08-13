using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.DataBases.Model;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Model.Datasets;
using SKitLs.Bots.Telegram.DataBases.Model.Messages;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Model;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using Settings = SKitLs.Bots.Telegram.DataBases.resources.settings.SkDBSettings;

namespace SKitLs.Bots.Telegram.DataBases
{
    /// <summary>
    /// The default realization of <see cref="IDataManager"/> that realizes all the basic functional.
    /// </summary>
    public sealed class DefaultDataManager : IDataManager
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => OnReflectiveCompile;

        public string? DatabaseLabel { get; set; }

        private IMenuManager MenuManager => Owner.ResolveService<IMenuManager>();

        /// <summary>
        /// The id used for main dataset that storages all other datasets.
        /// </summary>
        public static string SourceSetId => Settings.SourceSetId;
        /// <summary>
        /// The main dataset that storages all other datasets.
        /// </summary>
        public IBotDataSet<IBotDataSet> SourceSet { get; private init; }

        public DefaultDataManager(string? databaseLabel = null)
        {
            OpenDatabaseCallback = new(Settings.OpenDatabaseCallbackId, Settings.OpenDatabaseLK, Do_OpenDbPageAsync);
            OpenObjectCallback = new(Settings.OpenObjectCallbackId, Settings.OpenObjectLK, Do_OpenObjectAsync);
            AddNewCallback = new(Settings.AddNewCallbackId, Settings.AddNewLK, Do_AddProcessAsync);
            EditExistingCallback = new(Settings.EditExistingCallbackId, Settings.EditExistingLK, Do_EditProcessAsync);
            RemoveExistingCallback = new(Settings.RemoveExistingCallbackId, Settings.RemoveExistingLK, Do_RemoveAsync);

            DatabaseLabel = databaseLabel;
            var props = new DataSetProperties(SourceSetId, 0)
            {
                AllowAdd = false,
                AllowEdit = false,
                AllowExit = false,
                AllowRemove = false,
            };
            SourceSet = new BotDataSet<IBotDataSet>(SourceSetId, properties: props);
        }

        public List<IBotDataSet> GetAll() => SourceSet.GetAll();
        public async Task AddAsync(IBotDataSet dataSet) => await SourceSet.AddAsync(dataSet, null);
        public IBotDataSet GetSet(long setId) => TryGetSet(setId)
            ?? throw new NotDefinedException(this, typeof(IBotDataSet), setId.ToString());
        public IBotDataSet? TryGetSet(long setId) => (IBotDataSet?)SourceSet.TryGetExisting(setId);

        public IBotDataSet GetSet(string setNameId) => TryGetSet(setNameId)
            ?? throw new NotDefinedException(this, typeof(IBotDataSet), setNameId);
        public IBotDataSet? TryGetSet(string setNameId) => SourceSet.GetAllDisplayable()
            .Cast<IBotDataSet>()
            .ToList()
            .Find(x => x.DataSetId == setNameId);

        public IBotDataSet GetSet(Type setType) => TryGetSet(setType)
            ?? throw new NotDefinedException(this, typeof(IBotDataSet), setType.Name);
        public IBotDataSet? TryGetSet(Type setType) => SourceSet.GetAllDisplayable()
            .Cast<IBotDataSet>()
            .ToList()
            .Find(x => x.DataType == setType);

        public IBotDataSet<T> GetSet<T>() where T : class, IBotDisplayable => TryGetSet<T>()
            ?? throw new NotDefinedException(this, typeof(IBotDataSet), typeof(T).Name);
        public IBotDataSet<T>? TryGetSet<T>() where T : class, IBotDisplayable => (IBotDataSet<T>?)TryGetSet(typeof(T));

        public List<T> GetMergedData<T>() => SourceSet.GetAllDisplayable()
            .Cast<IBotDataSet>()
            .ToList()
            .FindAll(x => typeof(T).IsAssignableFrom(x.DataType))
            .SelectMany(x => x.GetAllDisplayable())
            .Cast<T>()
            .ToList();

        public IBotPage GetRootPage()
        {
            var display = new DataListMessage(SourceSet);
            var dataMenu = new DataSetsMenu(this);
            var dataPage = new WidgetPage(Settings.RootPage_Id, u => DatabaseLabel ?? u.Owner.ResolveBotString(Settings.RootPage_LabelLK), display, dataMenu);
            return dataPage;
        }
        public List<IBotAction> GetActionsContent() => new()
        {
            OpenDatabaseCallback,
            OpenObjectCallback,
            AddNewCallback,
            EditExistingCallback,
            RemoveExistingCallback,
        };

        public BotArgedCallback<PaginationInfo> OpenDatabaseCallback { get; }
        private async Task Do_OpenDbPageAsync(PaginationInfo args, SignedCallbackUpdate update)
        {
            var ds = args.DataSet;
            var context = ds.GetContextSubsetDisplayable(update);
            var display = new DataListMessage(context, ds.Properties.MainPageHeader, args);
            var dataMenu = new DataListMenu(this, context, args, ds.Properties.AllowAdd);
            var dataPage = new WidgetPage(Settings.DataTempPageId, ds.Properties.DataSetLabel, display, dataMenu);

            await MenuManager.PushPageAsync(dataPage, update);
        }

        public BotArgedCallback<ObjInfoArg> OpenObjectCallback { get; }
        private async Task Do_OpenObjectAsync(ObjInfoArg args, SignedCallbackUpdate update)
            => await args.DataSet.DisplayObjectDataAsync(args, update);
        public BotArgedCallback<PaginationInfo> AddNewCallback { get; }
        private async Task Do_AddProcessAsync(PaginationInfo args, SignedCallbackUpdate update)
            => await args.DataSet.LaunchAddProcessAsync(args, update);
        public BotArgedCallback<ObjInfoArg> EditExistingCallback { get; }
        private async Task Do_EditProcessAsync(ObjInfoArg args, SignedCallbackUpdate update)
            => await args.DataSet.LaunchEditProcessAsync(args, update);
        public BotArgedCallback<ObjInfoArg> RemoveExistingCallback { get; }
        private async Task Do_RemoveAsync(ObjInfoArg args, SignedCallbackUpdate update)
            => await args.DataSet.LaunchRemoveProcessAsync(args, update);
        
        private void OnReflectiveCompile(object sender, BotManager owner)
        {
            var dsRule = new ConvertRule<IBotDataSet>(id =>
            {
                var res = TryGetSet(id);
                return res is not null
                    ? ConvertResult<IBotDataSet>.OK(res)
                    : ConvertResult<IBotDataSet>.NotPresented();
            });
            owner.ResolveService<IArgsSerializeService>().AddRule(dsRule);

            GetActionsContent()
                .Where(x => x is DefaultCallback)
                .Cast<DefaultCallback>()
                .ToList()
                .ForEach(x => x.Label = Owner.ResolveBotString(x.Label));
        }
        public void ApplyTo(IStatefulActionManager<SignedMessageTextUpdate> entity)
        {
            // TODO
            if (Owner is null)
                throw new Exception($"Call {nameof(ApplyTo)} only after {nameof(BotBuilder.Build)}.");

            foreach (var ds in SourceSet.GetAll())
            {
                foreach (var proc in ds.DefinedProcesses)
                {
                    if (proc is IApplicant<IStatefulActionManager<SignedMessageTextUpdate>> applicant)
                    {
                        applicant.ApplyTo(entity);
                    }
                }
            }
        }
        public void ApplyTo(IStatefulActionManager<SignedCallbackUpdate> entity)
        {
            // TODO
            if (Owner is null)
                throw new Exception($"Call {nameof(ApplyTo)} only after {nameof(BotBuilder.Build)}.");

            entity.AddSafely(OpenDatabaseCallback);
            entity.AddSafely(AddNewCallback);
            entity.AddSafely(OpenObjectCallback);
            entity.AddSafely(EditExistingCallback);
            entity.AddSafely(RemoveExistingCallback);

            foreach (var ds in SourceSet.GetAll())
            {
                foreach (var proc in ds.DefinedProcesses)
                {
                    if (proc is IApplicant<IStatefulActionManager<SignedCallbackUpdate>> applicant)
                    {
                        applicant.ApplyTo(entity);
                    }
                }
            }
        }
        public void ApplyTo(IMenuManager entity)
        {
            entity.Define(GetRootPage());
        }
    }
}