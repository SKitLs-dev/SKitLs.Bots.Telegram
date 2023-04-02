using SKitLs.Bots.Telegram.Interactions.Prototype;

namespace SKitLs.Bots.Telegram.Interactions.Model
{
    public class BotTextInput : IBotTextInput
    {
        public int ExecutionWeight { get; set; }
        public SignedTextUpdatePredicate ShouldBeExecutedOn { get; set; }
        public SignedTextUpdate Executer { get; set; }

        public BotTextInput(SignedTextUpdatePredicate predication, SignedTextUpdate execution, int weight = 0) 
        {
            ExecutionWeight = weight;
            ShouldBeExecutedOn = predication;
            Executer = execution;
        }
    }
}
