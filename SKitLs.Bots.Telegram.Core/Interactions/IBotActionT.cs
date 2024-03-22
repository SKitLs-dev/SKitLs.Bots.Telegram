using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.Interactions
{
    /// <summary>
    /// An interface that defines typed bot actions with handling delegates for <typeparamref name="TUpdate"/> via <see cref="BotInteraction{TUpdate}"/>.
    /// <para/>
    /// This interface represents the <b>fifth level</b> of the bot's architecture.
    /// <list type="number">
    ///     <item/>
    ///     <item/>
    ///     <item/>
    ///     <item>
    ///         <term><see cref="IActionManager{TUpdate}"/></term>
    ///         <description>Provides complex logic for handling updates via interactions.</description>
    ///     </item>
    ///     <item>
    ///         <b><see cref="IBotAction"/></b>
    ///     </item>
    /// </list>
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this action should work with.</typeparam>
    public interface IBotAction<TUpdate> : IBotAction, IEquatable<IBotAction<TUpdate>> where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// An action delegate that should be executed when the action is triggered.
        /// </summary>
        public BotInteraction<TUpdate> Action { get; }

        /// <summary>
        /// Checks whether this action should be executed for a specific incoming update.
        /// </summary>
        /// <param name="update">The incoming update.</param>
        /// <returns><see langword="true"/> if this action should be executed; otherwise, <see langword="false"/>.</returns>
        public bool ShouldBeExecutedOn(TUpdate update);
    }
}