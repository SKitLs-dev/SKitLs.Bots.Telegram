using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Args
{
    public class PaginationInfo
    {
        [BotActionArgument(0)]
        public IBotDataSet DataSet { get; set; } = null!;

        [BotActionArgument(1)]
        public int StartIndex { get; set; } = 0;
        
        [BotActionArgument(2)]
        public int Count { get; set; } = 5;

        public PaginationInfo() { }
        public PaginationInfo(IBotDataSet dataSet, int startIndex = 0, int count = 5)
        {
            DataSet = dataSet;
            StartIndex = startIndex;
            Count = count;
        }

        public PaginationInfo Prev() => new()
        {
            DataSet = DataSet,
            StartIndex = StartIndex - Count > 0 ? StartIndex - Count : 0,
            Count = Count
        };
        public PaginationInfo Next() => new()
        {
            DataSet = DataSet,
            StartIndex = StartIndex + Count,
            Count = Count
        };
    }
}