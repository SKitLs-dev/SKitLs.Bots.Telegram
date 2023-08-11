using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    /// <summary>
    /// Represents a delegate used for building content based on the specified sender and update.
    /// </summary>
    /// <typeparam name="TSender">The type of the sender (usually a message or button).</typeparam>
    /// <param name="sender">The sender object that content is being built for.</param>
    /// <param name="update">The incoming casted update associated with the content building.</param>
    /// <returns>A task that represents the asynchronous operation and returns the sender object after content building.</returns>
    public delegate Task<TSender> ContentBuilder<TSender>(TSender sender, ICastedUpdate? update) where TSender : class;
}