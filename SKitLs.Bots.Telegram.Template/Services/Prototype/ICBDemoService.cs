using SKitLs.Bots.Telegram.Core.Services;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Template.Services.Prototype
{
    internal interface ICBDemoService : IBotService
    {
        public Task Run(SignedCallbackUpdate update);
    }
}
