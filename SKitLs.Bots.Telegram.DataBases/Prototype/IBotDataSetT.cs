using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.DataBases.Prototype
{
    public delegate Task OnDbObjUpdate<T>(T item, ICastedUpdate? update);

    public interface IBotDataSet<T> : IBotDataSet where T : class, IBotDisplayable
    {
        public event OnDbObjUpdate<T>? ObjectAdded;
        public event OnDbObjUpdate<T>? ObjectUpdated;
        public event OnDbObjUpdate<T>? ObjectRemoved;
        public event OnDbObjUpdate<T>? NewObjectRequested;

        public List<T> GetAllCasted();
        /// <summary>
        /// Mechanism of creating new instance of <typeparamref name="T"/>.
        /// </summary>
        /// <returns></returns>
        public Task<T> GetNewAsync(ICastedUpdate? trigger);
        /// <summary>
        /// Adds new data
        /// </summary>
        /// <param name="item"></param>
        public Task AddAsync(T item, ICastedUpdate? trigger);
        /// <summary>
        /// Removes data from list
        /// </summary>
        /// <param name="bid"></param>
        public Task RemoveAsync(long bid, ICastedUpdate? trigger);
        /// <summary>
        /// Removes data from list
        /// </summary>
        /// <param name="item"></param>
        public Task RemoveAsync(T item, ICastedUpdate? trigger);
        public Task UpdateAsync(T item, ICastedUpdate? trigger);
    }
}