using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Messages
{
    internal class DataListMenu : IPageMenu
    {
        private IDataManager Owner { get; set; }
        private List<IBotDisplayable> Data { get; set; }
        private int SourceSize { get; set; }
        private PaginationInfo Paging { get; set; }

        public DataListMenu(IDataManager owner, IBotDataSet dataSet) : this(owner, dataSet, dataSet.PaginationInfo) { }
        public DataListMenu(IDataManager owner, IBotDataSet dataSet, PaginationInfo paging) : this(owner, dataSet.Data, paging) { }
        public DataListMenu(IDataManager owner, List<IBotDisplayable> data, PaginationInfo paging)
        {
            Owner = owner;
            Paging = paging;
            SourceSize = data.Count;
            
            Data = new();
            if (paging.Count == 0) data.ForEach(x => Data.Add(x));
            else
            {
                int end = paging.StartIndex + paging.Count > data.Count
                    ? data.Count
                    : paging.StartIndex + paging.Count;
                for (int i = paging.StartIndex; i < end; i++)
                    Data.Add(data[i]);
            }
        }

        public IMesMenu Build(IPageWrap? previous, IPageWrap owner)
        {
            var serializer = Owner.Owner.ResolveService<IArgsSerilalizerService>();
            var res = new PairedInlineMenu()
            {
                ColumnsCount = 2
            };

            Data.ForEach(x => res.Add(x.ListLabel(),
                Owner.OpenObjCallabck.GetSerializedData(new(Paging, x.BotArgId), serializer), true));

            if (Paging.StartIndex > 0)
                res.Add("<<<", Owner.OpenCallabck.GetSerializedData(Paging.Prev(), serializer));
            if (Paging.StartIndex + Paging.Count < SourceSize)
                res.Add(">>>", Owner.OpenCallabck.GetSerializedData(Paging.Next(), serializer));
            if (previous is not null)
                res.Add("<< Назад", DefaultMenuManager.BuildBackCallback(previous), true);
            
            return res;
        }
    }
}