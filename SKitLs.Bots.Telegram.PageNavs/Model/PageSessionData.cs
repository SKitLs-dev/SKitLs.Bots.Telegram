using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.PageNavs.Pages;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// Represents session data used to store user's navigation history within a specific message with a defined ID.
    /// </summary>
    public sealed class PageSessionData
    {
        /// <summary>
        /// The user ID to which this session belongs.
        /// </summary>
        public long OwnerId { get; private init; }

        /// <summary>
        /// The message ID to which this session belongs.
        /// </summary>
        public int MessageId { get; private init; }

        /// <summary>
        /// The navigation history stack.
        /// </summary>
        public Stack<IBotPage> Route { get; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PageSessionData"/> class with the specified data.
        /// </summary>
        /// <param name="ownerId">The user ID to which this session belongs.</param>
        /// <param name="messageId">The message ID to which this session belongs.</param>
        public PageSessionData(long ownerId, int messageId)
        {
            OwnerId = ownerId;
            MessageId = messageId;
        }

        /// <summary>
        /// Pushes new page data to the session's history.
        /// </summary>
        /// <param name="page">The page data to push.</param>
        public void Push(IBotPage page) => Route.Push(page);

        /// <summary>
        /// Gets the latest page that the sender has opened without removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when the session doesn't contain any pages.</exception>
        public IBotPage Peek() => TryPeek(out IBotPage? res)
            ? res ?? throw new NotDefinedException(GetType(), typeof(IBotPage), nameof(Peek))
            : throw new NotDefinedException(GetType(), typeof(IBotPage), nameof(Peek));

        /// <summary>
        /// Tries to get the latest page that the sender has opened without removing it from the navigation history.
        /// </summary>
        /// <returns>The latest opened page or <see langword="null"/> if it doesn't exist.</returns>
        public bool TryPeek(out IBotPage? result) => Route.TryPeek(out result);

        /// <summary>
        /// Gets the latest page that the sender has opened, removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when the session doesn't contain any pages.</exception>
        public IBotPage Pop() => TryPop(out IBotPage? res)
            ? res ?? throw new NotDefinedException(GetType(), typeof(IBotPage), nameof(Pop))
            : throw new NotDefinedException(GetType(), typeof(IBotPage), nameof(Pop));

        /// <summary>
        /// Tries to get the latest page that the sender has opened, removing it from the navigation history.
        /// </summary>
        /// <returns>The latest opened page or <see langword="null"/> if it doesn't exist.</returns>
        public bool TryPop(out IBotPage? result) => Route.TryPop(out result);
    }
}