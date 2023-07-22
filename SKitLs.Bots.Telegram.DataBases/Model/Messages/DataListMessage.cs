using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Messages
{
    internal class DataListMessage : OutputMessageDecorText
    {
        public DataListMessage(IBotDataSet dataSet) : this(dataSet, dataSet.Pagination) { }
        public DataListMessage(IBotDataSet dataSet, PaginationInfo paging) : this(dataSet.GetAll(), paging, dataSet.Properties.AllowReadRows) { }
        public DataListMessage(IList<IBotDisplayable> data, bool allowReadRows) : this(data, new(), allowReadRows) { }
        public DataListMessage(IList<IBotDisplayable> data, PaginationInfo paging, bool allowReadRows) : base(BuildListBody(data, paging, allowReadRows))
        {
            Header = "База данных";
        }

        private static string BuildListBody(IList<IBotDisplayable> data, PaginationInfo pagination, bool allowReadRows)
        {
            // TODO
            if (pagination.StartIndex < 0) throw new Exception();
            if (pagination.Count > 20) throw new Exception();

            if (!allowReadRows)
            {
                return $"Всего записей: {data.Count}";
            }
            else
            {
                if (data.Count == 0) return "Данных пока что нет";

                string list = string.Empty;
                if (pagination.Count == 0) data.ToList().ForEach(x => list += $"• {x.ListDisplay()}\n");
                else
                {
                    int end = pagination.StartIndex + pagination.Count > data.Count
                        ? data.Count
                        : pagination.StartIndex + pagination.Count;
                    for (int i = pagination.StartIndex; i < end; i++)
                        list += $"• {data[i].ListDisplay()}\n";
                }
                return list;
            }
        }
    }
}