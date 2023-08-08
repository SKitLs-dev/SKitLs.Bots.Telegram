using SKitLs.Bots.Telegram.Core.Model.Interactions;

namespace SKitLs.Bots.Telegram.Core.Prototype
{
    // XML-Doc Update
    /// <summary>
    /// An interface that provides ways of collecting all <see cref="IBotAction"/>s,
    /// declared in a certain class.
    /// </summary>
    public interface IActionsHolder
    {
        /// <summary>
        /// Collects all <see cref="IBotAction"/>s declared in the class.
        /// </summary>
        /// <returns>Collected list of declared actions.</returns>
        public List<IBotAction> GetActionsContent();
    }
}