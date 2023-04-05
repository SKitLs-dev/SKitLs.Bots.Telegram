using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.ArgsInteraction.Interactions.Model
{
    public class BotArgTextInput<TArgument> : InteractionArgBase, IBotTextInput
    {
        public TArgument Arguments { get; set; }
        public int ExecutionWeight { get; set; }
        public SignedTextUpdatePredicate PredicateExecution { get; set; }
        public SignedTextUpdate Executer { get; set; }

        public BotArgTextInput(SignedTextUpdatePredicate predication, SignedTextUpdate execution,
            string? @base = null, int weight = 0) : base(@base)
        {
            ExecutionWeight = weight;
            PredicateExecution = predication;
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
