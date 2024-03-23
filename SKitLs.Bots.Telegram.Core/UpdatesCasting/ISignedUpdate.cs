using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Users;

namespace SKitLs.Bots.Telegram.Core.UpdatesCasting
{
    /// <summary>
    /// Represents a signed update, indicating an update that contains specific sender information.
    /// Inherits from <see cref="ICastedUpdate"/>.
    /// </summary>
    public interface ISignedUpdate : ICastedUpdate
    {
        /// <summary>
        /// Gets the sender instance associated with the update.
        /// </summary>
        /// <remarks>
        /// The sender instance is typically generated through mechanisms such as
        /// <see cref="ChatScanner.UsersManager"/> or <see cref="ChatScanner.GetDefaultBotUser"/>
        /// of a <see cref="ChatScanner"/> class by default.
        /// </remarks>
        public IBotUser Sender { get; }
    }
}