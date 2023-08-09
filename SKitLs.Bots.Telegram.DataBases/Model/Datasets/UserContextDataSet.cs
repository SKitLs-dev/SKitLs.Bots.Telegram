using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.DataBases.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Datasets
{
    public class UserContextDataSet<T> : ListDataSetBase<T> where T : class, IBotDisplayable, IOwnedData
    {
        public UserContextDataSet(string setId, IList<T>? data = null, string? dsLabel = null, Func<ICastedUpdate?, T>? createNew = null, DataSetProperties? properties = null)
            : base(setId, data, dsLabel, createNew, properties) { }

        public override List<IBotDisplayable> GetContextSubsetDisplayable(ISignedUpdate update)
            => GetUserSubsetDisplayable(update.Sender.TelegramId);
        public override List<IBotDisplayable> GetUserSubsetDisplayable(long telegramId) => GetUserSubset(telegramId)
            .Cast<IBotDisplayable>()
            .ToList();
        public override List<T> GetContextSubset(ISignedUpdate update) => GetUserSubset(update.Sender.TelegramId);
        public override List<T> GetUserSubset(long telegramId) => Data
            .Where(x => x.IsOwnedBy(telegramId))
            .ToList();
    }
}