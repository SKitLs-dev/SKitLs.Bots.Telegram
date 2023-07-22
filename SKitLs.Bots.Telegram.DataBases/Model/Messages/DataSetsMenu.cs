using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases.Model.Messages
{
    internal class DataSetsMenu : IPageMenu
    {
        private IDataManager Owner { get; set; }

        public DataSetsMenu(IDataManager owner) => Owner = owner;

        public IMesMenu Build(IBotPage? previous, IBotPage owner, ISignedUpdate update)
        {
            var mm = update.Owner.ResolveService<IMenuManager>();
            var serializer = Owner.Owner.ResolveService<IArgsSerilalizerService>();
            var res = new PairedInlineMenu();

            Owner.SourceSet.GetAll()
                .Cast<IBotDataSet>()
                .ToList()
                .ForEach(x => res.Add(x.ListLabel(), 
                Owner.OpenCallback.GetSerializedData(x.Pagination, serializer)));

            if (previous is not null)
                res.Add("<< Назад", mm.BackCallabck.GetSerializedData());
            return res;
        }
    }
}