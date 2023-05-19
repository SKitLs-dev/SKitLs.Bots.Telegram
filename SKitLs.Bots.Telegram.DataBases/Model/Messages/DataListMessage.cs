using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Messages
{
    internal class DataListMessage : OutputMessageDecorText
    {
        public DataListMessage(IBotDataSet dataSet) : this(dataSet.Data, dataSet.PaginationInfo) { }
        public DataListMessage(IBotDataSet dataSet, PaginationInfo paging) : this(dataSet.Data, paging) { }
        public DataListMessage(List<IBotDisplayable> data) : this(data, new()) { }
        public DataListMessage(List<IBotDisplayable> data, PaginationInfo paging) : base(BuildListBody(data, paging))
        {
            Header = "База данных";
        }

        private static string BuildListBody(List<IBotDisplayable> data, PaginationInfo paging)
        {
            // TODO
            if (paging.StartIndex < 0) throw new Exception();
            if (paging.Count > 20) throw new Exception();

            if (data.Count == 0) return "Данных пока что нет";

            string list = string.Empty;
            if (paging.Count == 0) data.ForEach(x => list += $"• {x.ListDisplay()}\n");
            else
            {
                int end = paging.StartIndex + paging.Count > data.Count
                    ? data.Count
                    : paging.StartIndex + paging.Count;
                for (int i = paging.StartIndex; i < end; i++)
                    list += $"• {data[i].ListDisplay()}\n";
            }
            return list;
        }
    }
}