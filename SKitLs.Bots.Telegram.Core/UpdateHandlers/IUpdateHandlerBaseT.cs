using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.Users;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    /// <summary>
    /// Generic interface defining common mechanisms for handling updates.
    /// <para/>
    /// This interface represents the <b>third level</b> of the bot's architecture.
    /// <list type="number">
    ///     <item/>
    ///     <item>
    ///         <term><see cref="ChatScanner"/></term>
    ///         <description>Used for handling updates in different chat types.</description>
    ///     </item>
    ///     <item>
    ///         <b><see cref="IUpdateHandlerBase"/></b>
    ///     </item>
    ///     <item>
    ///         <term><see cref="IActionManager{TUpdate}"/></term>
    ///         <description>Provides complex logic for handling updates via interactions.</description>
    ///     </item>
    /// </list>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>.
    /// </summary>
    /// <typeparam name="TUpdate">Specific casted update that this handler should work with.</typeparam>
    public interface IUpdateHandlerBase<TUpdate> : IUpdateHandlerBase where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Casts a common incoming <see cref="ICastedUpdate"/> to the specified
        /// <typeparamref name="TUpdate"/> update type.
        /// </summary>
        /// <param name="update">The update to handle.</param>
        /// <param name="sender">The sender to associate with the update.</param>
        /// <returns>The casted update of type <typeparamref name="TUpdate"/>.</returns>
        public TUpdate CastUpdate(ICastedUpdate update, IBotUser? sender);

        /// <summary>
        /// Asynchronously handles custom casted <typeparamref name="TUpdate"/> updates.
        /// <para>
        /// Casts and passes the update via the base <see cref="IUpdateHandlerBase.HandleUpdateAsync(ICastedUpdate, IBotUser?)"/>.
        /// </para>
        /// </summary>
        /// <param name="update">The update to handle.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task HandleUpdateAsync(TUpdate update);
    }
}