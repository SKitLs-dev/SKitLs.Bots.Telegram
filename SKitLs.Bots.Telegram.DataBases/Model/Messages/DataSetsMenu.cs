using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Messages
{
    internal class DataSetsMenu : IPageMenu
    {
        private IDataManager Owner { get; set; }

        public DataSetsMenu(IDataManager owner) => Owner = owner;

        public IMesMenu Build(IPageWrap? previous, IPageWrap owner)
        {
            var serializer = Owner.Owner.ResolveService<IArgsSerilalizerService>();
            var res = new PairedInlineMenu();

            Owner.SourceSet.Data
                .Cast<IBotDataSet>()
                .ToList()
                .ForEach(x => res.Add(x.ListLabel(), 
                Owner.OpenCallabck.GetSerializedData(x.PaginationInfo, serializer)));

            if (previous is not null)
                res.Add("<< Назад", DefaultMenuManager.BuildBackCallback(previous));
            return res;
        }
    }
}