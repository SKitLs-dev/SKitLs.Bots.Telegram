using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Messages
{
    internal class DataListMenu : IPageMenu
    {
        private IDataManager Owner { get; set; }
        private bool AllowAdd { get; set; }
        private List<IBotDisplayable> Data { get; set; }
        private int SourceSize { get; set; }
        private PaginationInfo Paging { get; set; }

        public DataListMenu(IDataManager owner, IBotDataSet dataSet) : this(owner, dataSet, dataSet.Pagination) { }
        public DataListMenu(IDataManager owner, IBotDataSet dataSet, PaginationInfo paging) : this(owner, dataSet.GetAll(), paging, dataSet.Properties.AllowAdd) { }
        public DataListMenu(IDataManager owner, IList<IBotDisplayable> data, PaginationInfo paging, bool allowAdd)
        {
            Owner = owner;
            Paging = paging;
            SourceSize = data.Count;
            AllowAdd = allowAdd;
            
            Data = new();
            if (paging.Count == 0) data.ToList().ForEach(x => Data.Add(x));
            else
            {
                int end = paging.StartIndex + paging.Count > data.Count
                    ? data.Count
                    : paging.StartIndex + paging.Count;
                for (int i = paging.StartIndex; i < end; i++)
                    Data.Add(data[i]);
            }
        }

        public IMesMenu Build(IBotPage? previous, IBotPage owner, ISignedUpdate update)
        {
            var mm  = Owner.Owner.ResolveService<IMenuManager>();
            var serializer = Owner.Owner.ResolveService<IArgsSerilalizerService>();
            var res = new PairedInlineMenu()
            {
                ColumnsCount = 2,
                Serializer = serializer,
            };

            Data.ForEach(x => res.Add(x.ListLabel(),
                Owner.OpenObjCallback.GetSerializedData(new(Paging, x.BotArgId), serializer), true));

            if (AllowAdd)
                res.Add(Owner.AddCallback, Paging);

            if (Paging.StartIndex > 0)
                res.Add("<<<", Owner.OpenCallback.GetSerializedData(Paging.Prev(), serializer));
            if (Paging.StartIndex + Paging.Count < SourceSize)
                res.Add(">>>", Owner.OpenCallback.GetSerializedData(Paging.Next(), serializer));
            if (previous is not null)
                res.Add("<< Назад", mm.BackCallabck.GetSerializedData(), true);
            
            return res;
        }
    }
}