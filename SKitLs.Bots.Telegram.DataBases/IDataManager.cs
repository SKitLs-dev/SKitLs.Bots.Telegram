using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model;
using SKitLs.Bots.Telegram.Core.Model.Builders;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.DataBases.Model.Args;
using SKitLs.Bots.Telegram.DataBases.Prototype;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.DataBases
{
    public interface IDataManager : IOwnerCompilable, IApplyable<IActionManager<SignedCallbackUpdate>>
    {
        public string SourceSetId { get; }
        public IBotDataSet SourceSet { get; }

        public IPageWrap GetRootPage();
        public BotArgedCallback<PaginationInfo> OpenCallabck { get; }
        public BotArgedCallback<PaginationInfo> AddCallabck { get; }
        public BotArgedCallback<ObjInfoArg> OpenObjCallabck { get; }
        public BotArgedCallback<ObjInfoArg> EditCallabck { get; }
        public BotArgedCallback<ObjInfoArg> RemoveCallabck { get; }
    }
}