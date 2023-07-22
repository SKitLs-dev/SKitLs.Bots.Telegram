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
    internal class ObjMenu : IPageMenu
    {
        public IDataManager Owner { get; private set; }
        public ObjInfoArg ObjInfo { get; private set; }
        public IBotDisplayable Object => ObjInfo.GetObject();

        public ObjMenu(IDataManager owner, ObjInfoArg infoArg)
        {
            Owner = owner;
            ObjInfo = infoArg;
        }

        public IMesMenu Build(IBotPage? previous, IBotPage owner, ISignedUpdate update)
        {
            var serializer = Owner.Owner.ResolveService<IArgsSerilalizerService>();
            var res = new PairedInlineMenu()
            {
                Serializer = serializer
            };

            if (ObjInfo.DataSet.Properties.AllowEdit)
                res.Add(Owner.EditCallback, ObjInfo);
            if (ObjInfo.DataSet.Properties.AllowRemove)
                res.Add(Owner.RemoveCallback, ObjInfo);
            res.Add("<< Назад", Owner.OpenCallback, ObjInfo.GetPagination());

            return res;
        }
    }
}