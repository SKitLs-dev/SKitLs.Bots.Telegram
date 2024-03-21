using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.Management
{
    // XML-Doc Update
    /// <summary>
    /// An interface that provides complex logic for handling updates via <see cref="IBotAction"/>
    /// interactions. Stores interactions and delegates incoming updates to stored actions.
    /// Provides simple architecture with one storage collection.
    /// <para>
    /// Fourth architecture level, addon for <see cref="IActionManager{TUpdate}"/>
    /// </para>
    /// <para>Inherits: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/></para>
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this manager should work with.</typeparam>
    public interface ILinearActionManager<TUpdate> : IActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// An internal storage used to store saved actions.
        /// </summary>
        public IList<IBotAction<TUpdate>> Actions { get; }
    }
}