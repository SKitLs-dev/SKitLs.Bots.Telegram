using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype
{
    /// <summary>
    /// An interface that defines a message entity that can be dynamically built based on an update.
    /// Classes implementing this interface can generate message content based on specific update data.
    /// </summary>
    public interface IBuildableMessage
    {
        /// <summary>
        /// Asynchronously builds the message content based on the provided <paramref name="update"/>.
        /// </summary>
        /// <param name="update">The update used to dynamically generate the message content.</param>
        /// <returns>An <see cref="ITelegramMessage"/> instance with dynamically generated content.</returns>
        public Task<ITelegramMessage> BuildContentAsync(ICastedUpdate update);
    }
}