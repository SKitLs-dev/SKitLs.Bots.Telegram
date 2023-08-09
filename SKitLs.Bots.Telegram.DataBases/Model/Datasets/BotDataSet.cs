using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.DataBases.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Datasets
{
    public class BotDataSet<T> : ListDataSetBase<T> where T : class, IBotDisplayable
    {
        public BotDataSet(string setId, IList<T>? data = null, string? dsLabel = null, Func<ICastedUpdate?, T>? createNew = null, DataSetProperties? properties = null)
            : base(setId, data, dsLabel, createNew, properties) { }

        public override List<IBotDisplayable> GetContextSubsetDisplayable(ISignedUpdate update) => GetAllDisplayable();
        public override List<IBotDisplayable> GetUserSubsetDisplayable(long telegramId) => throw new NotImplementedException();
        public override List<T> GetContextSubset(ISignedUpdate update) => GetAll();
        public override List<T> GetUserSubset(long telegramId) => throw new NotImplementedException();
    }
}