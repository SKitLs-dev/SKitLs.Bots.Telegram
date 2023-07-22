using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.DataBases.Model;
using SKitLs.Bots.Telegram.DataBases.Model.Args;

namespace SKitLs.Bots.Telegram.DataBases.Prototype
{
    public interface IBotDataSet : IArgPackable, IBotDisplayable
    {
        public IDataManager Owner { get; set; }

        /// <summary>
        /// Type of stored data
        /// </summary>
        public Type DataType { get; }
        /// <summary>
        /// Data set's Id
        /// </summary>
        public string DataSetId { get; }
        public DataSetProperties Properties { get; }
        public PaginationInfo Pagination { get; }
        public List<IBotProcess> DefinedProcesses { get; }

        /// <summary>
        /// Page used for displaying data. Use null for defaults.
        /// </summary>
        public void OverrideMainPage(IOutputMessage message);

        public List<IBotDisplayable> GetAll();
        /// <summary>
        /// Tries to get data by id
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public IBotDisplayable? TryGetExisting(long bid);
        /// <summary>
        /// Gets data by id
        /// </summary>
        /// <param name="bid"></param>
        /// <returns></returns>
        public IBotDisplayable GetExisting(long bid);

        public Task DisplayDataObjectAsync(ObjInfoArg args, SignedCallbackUpdate update);
        public Task LaunchAddProcessAsync(PaginationInfo args, SignedCallbackUpdate update);
        public Task LaunchEditProcessAsync(ObjInfoArg args, SignedCallbackUpdate update);
        public Task ExecuteRemoveAsync(ObjInfoArg args, SignedCallbackUpdate update);
    }
}