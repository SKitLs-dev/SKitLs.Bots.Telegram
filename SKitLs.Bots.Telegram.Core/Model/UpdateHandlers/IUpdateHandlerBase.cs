using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    // XML-Doc Update
    /// <summary>
    /// An interface that determines common mechanisms for updates handling.
    /// <para>
    /// Third architecture level.
    /// Upper: <see cref="ChatScanner"/>.
    /// Lower: <see cref="Management.IActionManager{TUpdate}"/>.
    /// </para>
    /// <para>
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>
    /// </para>
    /// </summary>
    public interface IUpdateHandlerBase : IOwnerCompilable, IBotActionsHolder
    {
        /// <summary>
        /// Handles <see cref="ICastedUpdate"/> updated, gotten from <see cref="ChatScanner"/>.
        /// </summary>
        /// <param name="update">Update to handle.</param>
        /// <param name="sender">Sender to sign update.</param>
        public Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender);
    }
}