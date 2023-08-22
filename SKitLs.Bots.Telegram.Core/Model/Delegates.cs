using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Prototype;

namespace SKitLs.Bots.Telegram.Core.Model
{
    /// <summary>
    /// A delegate representing an asynchronous interaction between a bot and an update of type <typeparamref name="TUpdate"/>.
    /// </summary>
    /// <typeparam name="TUpdate">The type of update parameter.</typeparam>
    /// <param name="update">The update used as input for the interaction.</param>
    /// <returns>An asynchronous task representing the bot's interaction with the update.</returns>
    public delegate Task BotInteraction<TUpdate>(TUpdate update) where TUpdate : ICastedUpdate;

    /// <summary>
    /// A delegate representing an asynchronous task that operates on an update of type <typeparamref name="TUpdate"/>
    /// and returns a result of type <typeparamref name="TOut"/>.
    /// </summary>
    /// <typeparam name="TSender">The type of the sender (usually a message or button).</typeparam>
    /// <typeparam name="TUpdate">The type of update parameter.</typeparam>
    /// <typeparam name="TOut">The type of the result returned by the task.</typeparam>
    /// <param name="sender">The sender object that content is being built for.</param>
    /// <param name="update">The update used as input for the task.</param>
    /// <returns>An asynchronous task that returns a result of type <typeparamref name="TOut"/>.</returns>
    public delegate Task<TOut> UpdateBasedTask<TSender, TUpdate, TOut>(TSender sender, TUpdate? update) where TUpdate : ICastedUpdate;

    /// <summary>
    /// A delegate representing an asynchronous event of user data change for an <see cref="IBotUser"/>.
    /// </summary>
    /// <param name="user">The user whose data has changed.</param>
    /// <param name="update">An incoming update.</param>
    /// <returns>An asynchronous task representing the handling of user data change.</returns>
    public delegate Task UserDataChanged(IBotUser user, ICastedUpdate? update);
}