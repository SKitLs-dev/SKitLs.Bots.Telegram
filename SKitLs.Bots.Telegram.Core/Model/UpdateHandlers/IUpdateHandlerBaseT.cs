using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    /// <summary>
    /// Generic interface that determines mechanisms for specific updates handling.
    /// <para>
    /// Implemented defaults are stored in <c>Core.Model.UpdateHandlers.Defaults</c> namespace.
    /// </para>
    /// <para>
    /// Supports inherited <see cref="IOwnerCompilable"/>, <see cref="IActionsHolder"/>
    /// </para>
    /// </summary>
    /// <typeparam name="TUpdate">Scecific casted update that this handler should work with.</typeparam>
    public interface IUpdateHandlerBase<TUpdate> : IUpdateHandlerBase where TUpdate : ICastedUpdate
    {
        /// <summary>
        /// Casts common incoming <see cref="ICastedUpdate"/> to the specified
        /// <typeparamref name="TUpdate"/> update type.
        /// </summary>
        /// <param name="update">Update to handle</param>
        /// <param name="sender">Sender to sign update</param>
        /// <returns>Casted updated oh a type <typeparamref name="TUpdate"/>.</returns>
        public TUpdate CastUpdate(ICastedUpdate update, IBotUser? sender);

        /// <summary>
        /// Handles custom casted <typeparamref name="TUpdate"/> updated.
        /// <para>
        /// Cast and pass update via base <see cref="IUpdateHandlerBase.HandleUpdateAsync(ICastedUpdate, IBotUser?)"/>
        /// </para>
        /// </summary>
        /// <param name="update">Update to handle</param>
        public Task HandleUpdateAsync(TUpdate update);
    }
}