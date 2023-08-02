using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.PageNavs.Prototype;

namespace SKitLs.Bots.Telegram.PageNavs.Model
{
    /// <summary>
    /// A session data used to store user's navigation history inside one certain message with defined id.
    /// </summary>
    public sealed class PageSessionData
    {
        /// <summary>
        /// User's id who this session belongs to.
        /// </summary>
        public long OwnerId { get; private init; }
        /// <summary>
        /// Message's id which this session belongs to.
        /// </summary>
        public int MessageId { get; private init; }
        /// <summary>
        /// Navigation history.
        /// </summary>
        public Stack<IBotPage> Route { get; } = new();

        /// <summary>
        /// Creates a new instance of a <see cref="PageSessionData"/> with specified data.
        /// </summary>
        /// <param name="ownerId">User's id who this session belongs to.</param>
        /// <param name="mesId">Message's id which this session belongs to.</param>
        public PageSessionData(long ownerId, int mesId)
        {
            OwnerId = ownerId;
            MessageId = mesId;
        }

        /// <summary>
        /// Pushes new page data to the session's history.
        /// </summary>
        /// <param name="page">Page data to push.</param>
        public void Push(IBotPage page) => Route.Push(page);
        /// <summary>
        /// Gets the latest page that sender has opened without removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when session doesn't contain any pages.</exception>
        public IBotPage Peek() => TryPeek(out IBotPage? res)
            ? res ?? throw new NotDefinedException(GetType(), typeof(IBotPage), nameof(Peek))
            : throw new NotDefinedException(GetType(), typeof(IBotPage), nameof(Peek));
        /// <summary>
        /// Tries to get the latest page that sender has opened without removing it from the navigation history.
        /// </summary>
        /// <returns>The latest opened page or <see langword="null"/> if it doesn't exist.</returns>
        public bool TryPeek(out IBotPage? result) => Route.TryPeek(out result);
        /// <summary>
        /// Gets the latest page that sender has opened, removing it from the navigation history.
        /// Throws an exception if that page doesn't exist.
        /// </summary>
        /// <returns>The latest opened page.</returns>
        /// <exception cref="NotDefinedException">Thrown when session doesn't contain any pages.</exception>
        public IBotPage Pop() => TryPop(out IBotPage? res)
            ? res ?? throw new NotDefinedException(GetType(), typeof(IBotPage), nameof(Pop))
            : throw new NotDefinedException(GetType(), typeof(IBotPage), nameof(Pop));
        /// <summary>
        /// Tries to get the latest page that sender has opened, removing it from the navigation history.
        /// </summary>
        /// <returns>The latest opened page or <see langword="null"/> if it doesn't exist.</returns>
        public bool TryPop(out IBotPage? result) => Route.TryPop(out result);
    }
}