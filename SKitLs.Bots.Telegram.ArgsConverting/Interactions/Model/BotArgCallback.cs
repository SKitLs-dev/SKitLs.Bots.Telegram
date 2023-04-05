using SKitLs.Bots.Telegram.Core.Prototypes;
using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.ArgsInteraction.Interactions.Model
{
    public class BotArgCallback : BotArgInteraction, IBotCallback
    {
        public BotCallbackAction Action { get; set; }
        public string Label { get; set; }

        public BotArgCallback(string @base, string label, BotCallbackAction action, int permissionLevel = 0,
            IOutputMessage? notEnoughRightsMes = null) : base(@base, permissionLevel, notEnoughRightsMes)
        {
            Action = action;
            Label = label;
        }

        public BotArgCallback ExtendLabel(string label)
        {
            Label += " " + label;
            return this;
        }

        public BotArgCallback WithArgsTypes(char? argsSplitter, params Type[] types)
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
            if (interaction is IBotCallback callback)
            {
                return callback.Base == Base;
            }
            else return false;
        }
    }
}