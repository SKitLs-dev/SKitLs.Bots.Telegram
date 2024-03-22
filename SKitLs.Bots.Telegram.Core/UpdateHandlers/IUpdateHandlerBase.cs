using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.Users;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model.UpdateHandlers
{
    /// <summary>
    /// An interface defining common mechanisms for handling updates.
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
    /// Supports: <see cref="IOwnerCompilable"/>, <see cref="IBotActionsHolder"/>
    /// </summary>
    public interface IUpdateHandlerBase : IOwnerCompilable, IBotActionsHolder
    {
        /// <summary>
        /// Asynchronously handles an incoming update obtained from <see cref="ChatScanner"/>.
        /// </summary>
        /// <param name="update">The update to handle.</param>
        /// <param name="sender">The sender to associate with the update.</param>
        public Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender);
    }
}