using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

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
        public string ObjId { get; set; }

        public ObjInfoArg() { }
        public ObjInfoArg(PaginationInfo source, string objId)
        {
            DataSet = source.DataSet;
            StartIndex = source.StartIndex;
            Count = source.Count;
            ObjId = objId;
        }

        public IBotDisplayable GetObject() => DataSet.GetData(ObjId);
        public PaginationInfo GetPagination() => new(DataSet, StartIndex, Count);
    }
}