using SKitLs.Bots.Telegram.Interactions.Prototype;
using System.Collections;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    public sealed class CommandsStateSection : InteractionStateSection, IEnumerable<IBotCommand>
    {
        public CommandsStateSection(string name) : base(name) => AvailableCommands = new();

        private List<IBotCommand> AvailableCommands { get; set; }
        //public bool AddCommandSafely(IBotCommand command)
        //{
        //    if (AvailableCommands.Find(c => c.IsSimilarWith(command) == null))
        //    {
        //        AvailableCommands.Add(command);
        //        return true;
        //    }
        //    else return false;
        //}
        //public int AddCommandsRangeSafly(List<IBotCommand> commands)
        //{
        //    int fault = 0;
        //    foreach (IBotCommand command in commands)
        //        if (!AddCommandSafely(command))
        //            fault++;
        //    return fault;
        //}
        public IEnumerator<IBotCommand> GetEnumerator()
        {
            foreach (var item in AvailableCommands)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static CommandsStateSection Hardcode(string name, List<IBotCommand> commands)
            => new(name) { AvailableCommands = commands };
    }
}