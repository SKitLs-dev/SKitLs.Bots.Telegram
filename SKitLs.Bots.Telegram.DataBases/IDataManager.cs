using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Prototype;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.DataBases
{
    /// <summary>
    /// A service that provides regular data regulation.
    /// Contains a reflective representation of storage data via <see cref="IBotDataSet"/>
    /// </summary>
    public interface IDataManager : IOwnerCompilable, IActionsHolder, IApplicant<IStatefulActionManager<SignedCallbackUpdate>>, IApplicant<IStatefulActionManager<SignedMessageTextUpdate>>, IApplicant<IMenuManager>
    {
        public List<IBotDataSet> GetAll();

        /// <summary>
        /// Adds new dataset to the manager.
        /// </summary>
        /// <param name="dataSet">Dataset to add.</param>
        public Task AddAsync(IBotDataSet dataSet);

        /// <summary>
        /// Gets storage dataset by its id.
        /// </summary>
        /// <param name="setId">Dataset's id.</param>
        /// <returns>Stored dataset.</returns>
        public IBotDataSet GetSet(long setId);
        public IBotDataSet? TryGetSet(long setId);

        /// <summary>
        /// Gets stored dataset by its id.
        /// </summary>
        /// <param name="setNameId">Dataset's id.</param>
        /// <returns>Stored dataset.</returns>
        public IBotDataSet GetSet(string setNameId);
        public IBotDataSet? TryGetSet(string setNameId);

        /// <summary>
        /// Gets stored dataset by a type of the collected data.
        /// </summary>
        /// <param name="setType">Dataset's collected data type.</param>
        /// <returns>stored dataset.</returns>
        public IBotDataSet GetSet(Type setType);
        public IBotDataSet? TryGetSet(Type setType);

        /// <summary>
        /// Gets casted stored dataset by a type of the collected data.
        /// </summary>
        /// <typeparam name="T">Dataset's collected data type</typeparam>
        /// <returns>stored dataset.</returns>
        public IBotDataSet<T> GetSet<T>() where T : class, IBotDisplayable;
        public IBotDataSet<T>? TryGetSet<T>() where T : class, IBotDisplayable;

        /// <summary>
        /// Gets the casted list of all collected data that is assignable from the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Datasets' types should be assignable from this type</typeparam>
        /// <returns>Merged list of all casted data.</returns>
        public List<T> GetMergedData<T>();

        /// <summary>
        /// The main page of the dataset manager, that should be mounted to one of the branches of <see cref="IMenuManager"/>
        /// used in your project. See summary's example.
        /// </summary>
        /// <returns>The main page of the data manager.</returns>
        /// <example>
        /// var mainMenu = new PageNavMenu();
        /// var mainPage = new StaticPage(..., mainMenu);
        /// var dataControlPage = _dm.GetRootPage();
        /// mainMenu.PathTo(dataControlPage);
        /// </example>
        public IBotPage GetRootPage();

        /// <summary>
        /// Default callback that opens datasets pages, including <see cref="GetRootPage"/>.
        /// </summary>
        public BotArgedCallback<PaginationInfo> OpenDatabaseCallback { get; }
        /// <summary>
        /// Default callback that provides 
        /// </summary>
        public BotArgedCallback<PaginationInfo> AddNewCallback { get; }
        /// <summary>
        /// Default callback that
        /// </summary>
        public BotArgedCallback<ObjInfoArg> OpenObjectCallback { get; }
        /// <summary>
        /// Default callback that
        /// </summary>
        public BotArgedCallback<ObjInfoArg> EditExistingCallback { get; }
        /// <summary>
        /// Default callback that
        /// </summary>
        public BotArgedCallback<ObjInfoArg> RemoveExistingCallback { get; }
    }
}