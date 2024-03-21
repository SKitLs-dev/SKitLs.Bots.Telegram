using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Template.Services.Prototype
{
    internal interface ICBDemoService : IOwnerCompilable
    {
        public Task Run(SignedCallbackUpdate update);
    }
}
