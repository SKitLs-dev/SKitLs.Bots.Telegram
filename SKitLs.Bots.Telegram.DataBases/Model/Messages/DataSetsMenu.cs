using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus.Inline;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
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

        public async Task<IMessageMenu> BuildAsync(IBotPage? previous, IBotPage owner, ISignedUpdate update)
        {
            var mm = update.Owner.ResolveService<IMenuManager>();
            var serializer = Owner.Owner.ResolveService<IArgsSerializeService>();
            var res = new InlineMenu();

            Owner.GetAll()
                .ToList()
                .ForEach(x => res.Add(x.ListLabel(), 
                Owner.OpenDatabaseCallback.GetSerializedData(x.Pagination, serializer)));

            if (previous is not null)
                res.Add("<< Назад", mm.BackCallback.GetSerializedData());
            return await res.BuildContentAsync(update);
        }
    }
}