using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Core.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Management
{
    /// <summary>
    /// An interface that provides complex logic for handling updates via interactions with <see cref="IBotAction"/>s.
    /// Stores interactions and delegates incoming updates to stored actions.
    /// Provides a simple architecture with one storage collection.
    /// <para/>
    /// This interface represents the <b>fourth level</b> of the bot's architecture.
    /// <list type="number">
    ///     <item/>
    ///     <item/>
    ///     <item>
    ///         <term><see cref="IUpdateHandlerBase"/></term>
    ///         <description>Provides common mechanisms for updates handling.</description>
    ///     </item>
    ///     <item>
    ///         <b><see cref="IActionManager{TUpdate}"/></b>
    ///     </item>
    ///     <item>
    ///         <term><see cref="IBotAction"/></term>
    ///         <description>Provides a generic definition for bot actions.</description>
    ///     </item>
    /// </list>
    /// Supports: <see cref="IDebugNamed"/>, <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>.
    /// </summary>
    /// <typeparam name="TUpdate">The specific casted update type that this manager should work with.</typeparam>
    public interface ILinearActionManager<TUpdate> : IActionManager<TUpdate> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// An internal storage used to store saved actions.
        /// </summary>
        public IList<IBotAction<TUpdate>> Actions { get; }
    }
}