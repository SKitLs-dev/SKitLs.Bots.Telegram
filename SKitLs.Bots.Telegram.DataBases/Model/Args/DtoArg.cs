using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.DataBases.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Args
{
    public class DtoArg<T> where T : IBotDisplayable
    {
        [BotActionArgument(0)]
        public IBotDataSet DataSet { get; set; } = null!;

        [BotActionArgument(1)]
        public long DataId { get; set; }

        public DtoArg() { }
        public DtoArg(T obj, IBotDataSet ds)
        {
            DataSet = ds;
            DataId = obj.BotArgId;
            if (ds.GetExisting(DataId) is null) throw new ArgumentException(nameof(obj));
        }

        public T GetValue() => (T)DataSet.GetExisting(DataId);
    }
}