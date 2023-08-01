using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.Management
{
    /// <summary>
    /// An interface that provides complex logic for handling updates via <see cref="IBotAction"/>
    /// interactions. Stores interactions and delegates incoming updates to stored actions.
    /// <para>
    /// Fourth architecture level.
    /// Upper: <see cref="UpdateHandlers.IUpdateHandlerBase"/>.
    /// Lower: <see cref="IBotAction"/>.
    /// </para>
    /// <para>Supports: <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/></para>
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this manager should work with.</typeparam>
    public interface IActionManager<TUpdate> : IDebugNamed, IOwnerCompilable, IActionsHolder where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Safely adds new action to internal storage,
        /// verifying it is unique.
        /// </summary>
        /// <param name="action">Action to be stored.</param>
        public void AddSafely(IBotAction<TUpdate> action);

        /// <summary>
        /// Safely adds range of actions to internal storage,
        /// verifying they are unique.
        /// </summary>
        /// <param name="actions">Actions to be stored.</param>
        public void AddRangeSafely(ICollection<IBotAction<TUpdate>> actions);

        /// <summary>
        /// Manages incoming update, delegating it to one of a stored actions.
        /// </summary>
        /// <param name="update">Update to be handled.</param>
        public Task ManageUpdateAsync(TUpdate update);
    }
}