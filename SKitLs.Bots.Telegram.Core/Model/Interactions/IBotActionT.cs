using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    /// <summary>
    /// An interface that provides typed definition for bot actions with
    /// <typeparamref name="TUpdate"/> handling delegate via
    /// <see cref="BotInteraction{TUpdate}"/>.
    /// <para>
    /// Fifth architecture level.
    /// Upper: <see cref="Management.IActionManager{TUpdate}"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this action should work with.</typeparam>
    public interface IBotAction<TUpdate> : IBotAction, IEquatable<IBotAction<TUpdate>> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// An action that should be raised on action execution.
        /// </summary>
        public BotInteraction<TUpdate> Action { get; }
        /// <summary>
        /// Checks either this action should be executed on a certain incoming update.
        /// </summary>
        /// <param name="update">An incoming update</param>
        /// <returns><see langword="true"/> if this action should be executed; otherwise, <see langword="false"/>.</returns>
        public bool ShouldBeExecutedOn(TUpdate update);
    }
}