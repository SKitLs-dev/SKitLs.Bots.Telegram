using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    /// <summary>
    /// Represents an interface for building the content of an entity asynchronously.
    /// </summary>
    /// <typeparam name="T">Specific type of the instance that should be returned after content is built.</typeparam>
    public interface IBuildableContent<T>
    {
        /// <summary>
        /// Asynchronously constructs the content of the entity based on the given update.
        /// </summary>
        /// <param name="update">The incoming casted update associated with the menu construction.</param>
        /// <returns>A task that represents the asynchronous operation and returns the built <typeparamref name="T"/>.</returns>
        public Task<T> BuildContentAsync(ICastedUpdate? update);
    }
}