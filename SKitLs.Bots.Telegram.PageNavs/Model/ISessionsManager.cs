using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Users;
using SKitLs.Bots.Telegram.PageNavs.Pages;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    // XML-Doc
    /// <summary>
    /// Manages sessions and navigation between pages in the bot's interface.
    /// </summary>
    public interface ISessionsManager
    {
        /// <inheritdoc cref="CheckSession(SignedCallbackUpdate)"/>
        public bool CheckSession(ISignedUpdate update);

        /// <inheritdoc cref="CheckSession(long, int)"/>
        /// <param name="update">The incoming signed update.</param>
        public bool CheckSession(SignedCallbackUpdate update);

        /// <summary>
        /// Checks whether a session for the specified user and message exists and is valid.
        /// </summary>
        /// <param name="userId">The user's ID to check the session for.</param>
        /// <param name="messageId">The message's ID to check the session for.</param>
        /// <returns><see langword="true"/> if the session exists and is valid; otherwise, <see langword="false"/>.</returns>
        public bool CheckSession(long userId, int messageId);

        /// <inheritdoc cref="GetOrInitSession(long, int)"/>
        /// <param name="update">The incoming signed update.</param>
        /// <param name="messageId">The ID of the message associated with the session.</param>
        public PageSessionData GetOrInitSession(ISignedUpdate update, int messageId);

        /// <inheritdoc cref="GetOrInitSession(long, int)"/>
        /// <param name="sender">The sender whose session needs to be retrieved or initialized.</param>
        /// <param name="messageId">The ID of the message associated with the session.</param>
        public PageSessionData GetOrInitSession(IBotUser sender, int messageId);

        /// <summary>
        /// Gets the user's menu session by their ID for a certain message. If it doesn't exist, generates and returns a new one.
        /// </summary>
        /// <param name="senderId">The user's ID.</param>
        /// <param name="messageId">The ID of the message associated with the session.</param>
        /// <returns>An existing or a new <see cref="PageSessionData"/> for the specified user and message.</returns>
        public PageSessionData GetOrInitSession(long senderId, int messageId);

        /// <summary>
        /// Refreshes the session for the specified user.
        /// </summary>
        /// <param name="senderId">The ID of the user whose session needs to be refreshed.</param>
        public void RefreshSession(long senderId);

        /// <inheritdoc cref="Push(IBotPage, long, int)"/>
        /// <param name="page">The page to push.</param>
        /// <param name="session">The session to push page to.</param>
        public void Push(IBotPage page, PageSessionData session);

        /// <inheritdoc cref="Push(IBotPage, IBotUser, int)"/>
        /// <param name="page">The page to push.</param>
        /// <param name="update">The incoming signed update.</param>
        /// <param name="messageId">A message that host menu id.</param>
        public void Push(IBotPage page, ISignedUpdate update, int messageId);

        /// <inheritdoc cref="Push(IBotPage, long, int)"/>
        /// <param name="page">The page to push.</param>
        /// <param name="sender">The sender whose session needs to be retrieved or initialized.</param>
        /// <param name="messageId">A message that host menu id.</param>
        public void Push(IBotPage page, IBotUser sender, int messageId);

        /// <summary>
        /// Pushes the <paramref name="page"/> to a certain user's navigation data.
        /// </summary>
        /// <param name="page">The page to push.</param>
        /// <param name="senderId">The ID of the user whose session needs to be updated.</param>
        /// <param name="messageId">A message that host menu id.</param>
        public void Push(IBotPage page, long senderId, int messageId);

        /// <inheritdoc cref="TryPeek(ISignedUpdate)"/>
        public IBotPage Peek(ISignedUpdate update);

        /// <inheritdoc cref="TryPeek(IBotUser)"/>
        public IBotPage Peek(IBotUser sender);

        /// <inheritdoc cref="TryPeek(long)"/>
        public IBotPage Peek(long senderId);

        /// <inheritdoc cref="TryPeek(IBotUser)"/>
        /// <param name="update">The incoming signed update.</param>
        public IBotPage? TryPeek(ISignedUpdate update);

        /// <inheritdoc cref="TryPeek(long)"/>
        /// <param name="sender">The sender whose session needs to be retrieved.</param>
        public IBotPage? TryPeek(IBotUser sender);

        /// <summary>
        /// Retrieves the latest page that the sender has opened without removing it from the navigation history.
        /// </summary>
        /// <returns>The latest page opened by the sender.</returns>
        /// <param name="senderId">The ID of the sender.</param>
        public IBotPage? TryPeek(long senderId);

        /// <inheritdoc cref="TryPop(ISignedUpdate)"/>
        /// <param name="update">The incoming signed update.</param>
        public IBotPage Pop(ISignedUpdate update);

        /// <inheritdoc cref="TryPop(IBotUser)"/>
        /// <param name="sender">The sender whose session needs to be retrieved or initialized.</param>
        public IBotPage Pop(IBotUser sender);

        /// <inheritdoc cref="TryPop(long)"/>
        public IBotPage Pop(long senderId);

        /// <inheritdoc cref="TryPop(long)"/>
        /// <param name="update">The incoming signed update.</param>
        public IBotPage? TryPop(ISignedUpdate update);

        /// <inheritdoc cref="TryPop(long)"/>
        /// <param name="sender">The sender whose session needs to be retrieved or initialized.</param>
        public IBotPage? TryPop(IBotUser sender);

        /// <summary>
        /// Gets the latest page that the sender has opened, removing it from the navigation history.
        /// </summary>
        /// <param name="senderId">The ID of the sender.</param>
        /// <returns>The latest page opened by the sender.</returns>
        public IBotPage? TryPop(long senderId);
    }
}