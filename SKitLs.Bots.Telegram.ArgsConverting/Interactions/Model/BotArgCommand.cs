using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Interactions.Model
{
    public class BotArgCommand : BotArgInteraction, IBotCommand
    {
        public BotCommandAction Action { get; set; }

        public BotArgCommand(string @base, BotCommandAction action, int permissionLevel = 0,
            IOutputMessage? notEnoughRightsMes = null) : base(@base, permissionLevel, notEnoughRightsMes)
        {
            Action = action;
        }

        public BotArgCommand WithArgsTypes(char? argsSplitter, params Type[] types)
        {
            if (argsSplitter == null && types.Length > 1)
                throw new ArgumentNullException(nameof(argsSplitter));

            ArgsSplitter = argsSplitter;
            foreach (var argType in types)
                ArgsType.Add(argType);

            return this;
        }
        public bool IsSimilarWith(IBotInteraction interaction)
        {
            if (interaction is IBotCommand command)
            {
                return command.Base == Base;
            }
            else return false;
        }
    }
}