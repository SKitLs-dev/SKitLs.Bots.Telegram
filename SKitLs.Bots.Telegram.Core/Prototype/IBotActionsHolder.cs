using SKitLs.Bots.Telegram.Core.Model.Interactions;

namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// An interface that facilitates the collection of all <see cref="IBotAction"/> instances declared within a specific class.
    /// </summary>
    public interface IBotActionsHolder
    {
        /// <summary>
        /// Collects all <see cref="IBotAction"/> instances declared in the implementing class.
        /// </summary>
        /// <returns>A list of all declared actions collected from the implementing class.</returns>
        public List<IBotAction> GetHeldActions();
    }
}