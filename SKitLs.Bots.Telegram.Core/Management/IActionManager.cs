using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdateHandlers;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.Management
{
    /// <summary>
    /// An interface that provides complex logic for handling updates via interactions with <see cref="IBotAction"/>s.
    /// Stores interactions and delegates incoming updates to stored actions.
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
    public interface IActionManager<TUpdate> : IDebugNamed, IOwnerCompilable, IBotActionsHolder where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Safely adds a new action to the internal storage.
        /// Verifies it is unique via <see cref="IBotAction.ActionId"/>.
        /// </summary>
        /// <param name="action">The action to be stored.</param>
        public void AddSafely(IBotAction<TUpdate> action);

        /// <summary>
        /// Safely adds a range of actions to the internal storage.
        /// Verifies they are unique via <see cref="IBotAction.ActionId"/>.
        /// </summary>
        /// <param name="actions">The actions to be stored.</param>
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions);

        /// <summary>
        /// Manages incoming update, delegating it to one of the stored actions.
        /// </summary>
        /// <param name="update">The update to be handled.</param>
        public Task ManageUpdateAsync(TUpdate update);
    }
}