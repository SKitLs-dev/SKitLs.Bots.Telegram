using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Users;
using SKitLs.Bots.Telegram.PageNavs.Pages;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <inheritdoc cref="ISessionsManager"/>
    public class SessionsManager : ISessionsManager
    {
        /// <summary>
        /// An internal storage that provides access to all saved navigation sessions.
        /// </summary>
        private Dictionary<long, PageSessionData> UsersSessions { get; } = [];

        /// <inheritdoc/>
        public bool CheckSession(ISignedUpdate update) => update is SignedCallbackUpdate callback && CheckSession(callback);

        /// <inheritdoc/>
        public bool CheckSession(SignedCallbackUpdate update) => CheckSession(update.Sender.TelegramId, update.TriggerMessageId);

        /// <inheritdoc/>
        public bool CheckSession(long userId, int messageId) => UsersSessions.ContainsKey(userId) && UsersSessions[userId].MessageId == messageId;

        /// <inheritdoc/>
        public PageSessionData GetOrInitSession(ISignedUpdate update, int messageId) => GetOrInitSession(update.Sender, messageId);

        /// <inheritdoc/>
        public PageSessionData GetOrInitSession(IBotUser sender, int messageId) => GetOrInitSession(sender.TelegramId, messageId);

        /// <inheritdoc/>
        public PageSessionData GetOrInitSession(long senderId, int messageId)
        {
            if (CheckSession(senderId, messageId))
                return UsersSessions[senderId];

            var res = new PageSessionData(senderId, messageId);
            UsersSessions.TryAdd(senderId, res);
            if (UsersSessions[senderId].MessageId != messageId)
                UsersSessions[senderId] = res;
            return res;
        }

        /// <inheritdoc/>
        public void RefreshSession(long senderId) => UsersSessions.Remove(senderId);

        /// <inheritdoc/>
        public void Push(IBotPage page, PageSessionData session) => Push(page, session.OwnerId, session.MessageId);

        /// <inheritdoc/>
        public void Push(IBotPage page, ISignedUpdate update, int messageId) => Push(page, update.Sender, messageId);

        /// <inheritdoc/>
        public void Push(IBotPage page, IBotUser sender, int messageId) => Push(page, sender.TelegramId, messageId);

        /// <inheritdoc/>
        public void Push(IBotPage page, long senderId, int messageId) => GetOrInitSession(senderId, messageId).Push(page);

        /// <inheritdoc/>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/> for sender doesn't exist.</exception>
        public IBotPage Peek(ISignedUpdate update) => Peek(update.Sender);

        /// <inheritdoc/>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/> for sender doesn't exist.</exception>
        public IBotPage Peek(IBotUser sender) => Peek(sender.TelegramId);

        /// <inheritdoc/>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/> for sender doesn't exist.</exception>
        public IBotPage Peek(long senderId) => TryPeek(senderId) ?? throw new NotDefinedException(this, typeof(PageSessionData), senderId.ToString());

        /// <inheritdoc/>
        public IBotPage? TryPeek(ISignedUpdate update) => TryPeek(update.Sender);

        /// <inheritdoc/>
        public IBotPage? TryPeek(IBotUser sender) => TryPeek(sender.TelegramId);

        /// <inheritdoc/>
        public IBotPage? TryPeek(long senderId)
        {
            if (UsersSessions.TryGetValue(senderId, out PageSessionData? value))
                if (value.TryPeek(out IBotPage? res))
                    return res;
            return null;
        }

        /// <inheritdoc/>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/> for sender doesn't exist.</exception>
        public IBotPage Pop(ISignedUpdate update) => Pop(update.Sender);

        /// <inheritdoc/>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/> for sender doesn't exist.</exception>
        public IBotPage Pop(IBotUser sender) => Pop(sender.TelegramId);

        /// <inheritdoc/>
        /// <exception cref="NotDefinedException">Thrown when <see cref="PageSessionData"/> for sender doesn't exist.</exception>
        public IBotPage Pop(long senderId) => TryPop(senderId) ?? throw new NotDefinedException(this, typeof(PageSessionData), senderId.ToString());

        /// <inheritdoc/>
        public IBotPage? TryPop(ISignedUpdate update) => TryPop(update.Sender);

        /// <inheritdoc/>
        public IBotPage? TryPop(IBotUser sender) => TryPop(sender.TelegramId);

        /// <inheritdoc/>
        public IBotPage? TryPop(long senderId)
        {
            if (UsersSessions.TryGetValue(senderId, out PageSessionData? value))
                if (value.TryPop(out IBotPage? res))
                    return res;
            return null;
        }
    }
}