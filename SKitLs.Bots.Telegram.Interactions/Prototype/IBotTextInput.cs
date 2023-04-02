using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.Interactions.Prototype
{
    public delegate bool SignedTextUpdatePredicate(SignedMessageTextUpdate update);
    public delegate Task SignedTextUpdate(IBotTextInput inputRule, SignedMessageTextUpdate update);

    public interface IBotTextInput : IBotInteraction
    {
        public int ExecutionWeight { get; }
        public SignedTextUpdatePredicate ShouldBeExecutedOn { get; }
        public SignedTextUpdate Executer { get; }
    }
}
