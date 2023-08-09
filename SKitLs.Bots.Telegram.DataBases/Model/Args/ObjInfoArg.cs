using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.DataBases.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Args
{
    public class ObjInfoArg
    {
        [BotActionArgument(0)]
        public IBotDataSet DataSet { get; set; } = null!;

        [BotActionArgument(1)]
        public int StartIndex { get; set; } = 0;

        [BotActionArgument(2)]
        public int Count { get; set; } = 5;

        [BotActionArgument(3)]
        public long ObjId { get; set; }

        public ObjInfoArg() { }
        internal ObjInfoArg(PaginationInfo source, long objId)
        {
            DataSet = source.DataSet;
            StartIndex = source.StartIndex;
            Count = source.Count;
            ObjId = objId;
        }
        public ObjInfoArg(IBotDataSet dataSet, long objId, int startIndex = 0, int count = 5)
        {
            DataSet = dataSet;
            StartIndex = startIndex;
            Count = count;
            ObjId = objId;
        }

        public IBotDisplayable GetObject() => DataSet.GetExisting(ObjId);
        // TODO type equality: T and DataSet.Type
        public T GetObject<T>() => (T)DataSet.GetExisting(ObjId);
        public PaginationInfo GetPagination() => new(DataSet, StartIndex, Count);
    }
}