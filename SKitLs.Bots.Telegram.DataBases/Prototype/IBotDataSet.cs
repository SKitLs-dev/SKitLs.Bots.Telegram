using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.DataBases.Model;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Prototype
{
    public interface IBotDataSet : IOwnerCompilable, IArgPackable, IBotDisplayable
    {
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

        public int Count { get; }

        public List<IBotDisplayable> GetContextSubsetDisplayable(ISignedUpdate update);
        public List<IBotDisplayable> GetUserSubsetDisplayable(long telegramId);
        public IBotDisplayable FirstDisplayable();

        public List<IBotDisplayable> GetAllDisplayable();
        /// <summary>
        /// Tries to get data by id
        /// </summary>
        /// <param name="bid">.</param>
        /// <returns></returns>
        public IBotDisplayable? TryGetExisting(long bid);
        /// <summary>
        /// Gets data by id
        /// </summary>
        /// <param name="bid">.</param>
        /// <returns></returns>
        public IBotDisplayable GetExisting(long bid);

        public Task DisplayObjectDataAsync(ObjInfoArg args, SignedCallbackUpdate update);
        public Task LaunchAddProcessAsync(PaginationInfo args, SignedCallbackUpdate update);
        public Task LaunchEditProcessAsync(ObjInfoArg args, SignedCallbackUpdate update);
        public Task LaunchRemoveProcessAsync(ObjInfoArg args, SignedCallbackUpdate update);
        public string ResolveStatus(ProcessCompleteStatus status, DbActionType action);
    }
}