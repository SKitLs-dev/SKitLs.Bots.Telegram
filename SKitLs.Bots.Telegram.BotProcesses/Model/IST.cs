using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model
{
    public class IST
    {
        public string Id { get; set; }
        public IUserState State { get; set; }
        public string TerminationalKey { get; set; }

        public IST(string id, IUserState state, string terminationalKey)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            State = state ?? throw new ArgumentNullException(nameof(state));
            TerminationalKey = terminationalKey ?? throw new ArgumentNullException(nameof(terminationalKey));
        }

        public static IST Dynamic() => new("process.dynamic", new DefaultUserState(-100, "dynamic"), "dynamic");
    }
}