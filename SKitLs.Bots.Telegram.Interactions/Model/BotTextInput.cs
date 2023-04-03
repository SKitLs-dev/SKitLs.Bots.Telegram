using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Interactions.Model
{
    public class BotTextInput : IBotTextInput
    {
        public string Base => throw new NotImplementedException();
        public int ExecutionWeight { get; set; }
        public SignedTextUpdatePredicate PredicateExecution { get; set; }
        public SignedTextUpdate Executer { get; set; }

        public BotTextInput(SignedTextUpdatePredicate predication, SignedTextUpdate execution, int weight = 0) 
        {
            ExecutionWeight = weight;
            PredicateExecution = predication;
            Executer = execution;
        }

        public bool ShouldBeExecutedOn(string message);
        public bool IsSimilarWith(IBotInteraction interaction)
        {
            throw new NotImplementedException();
        }
    }
}
