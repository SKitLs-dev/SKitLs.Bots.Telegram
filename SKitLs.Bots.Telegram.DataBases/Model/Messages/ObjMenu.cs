using SKitLs.Bots.Telegram.AdvancedMessages.Model.Menus;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;
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

        public IMesMenu Build(IPageWrap? previous, IPageWrap owner)
        {
            var serializer = Owner.Owner.ResolveService<IArgsSerilalizerService>();
            var res = new PairedInlineMenu();

            res.Add(Owner.EditCallabck.Label, Owner.EditCallabck.GetSerializedData(ObjInfo, serializer));
            res.Add(Owner.RemoveCallabck.Label, Owner.RemoveCallabck.GetSerializedData(ObjInfo, serializer));
            res.Add("<< Назад", Owner.OpenCallabck.GetSerializedData(ObjInfo.GetPagination(), serializer));

            return res;
        }
    }
}