using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Interactions.Model
{
    public class BotArgTextInput : InteractionArgBase, IBotTextInput
    {
        public int ExecutionWeight { get; set; }
        public SignedTextUpdatePredicate ShouldBeExecutedOn { get; set; }
        public SignedTextUpdate Executer { get; set; }

        public BotArgTextInput(SignedTextUpdatePredicate predication, SignedTextUpdate execution,
            string? @base = null, int weight = 0) : base(@base)
        {
            ExecutionWeight = weight;
            ShouldBeExecutedOn = predication;
            Executer = execution;
        }

        public BotArgTextInput WithArgsTypes(char? argsSplitter, params Type[] types)
        {
            if (argsSplitter == null && types.Length > 1)
                throw new ArgumentNullException(nameof(argsSplitter));

            ArgsSplitter = argsSplitter;
            foreach (var argType in types)
                ArgsType.Add(argType);

            return this;
        }
    }
}
