using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype
{
    /// <summary>
    /// Represents a delegate for actions that support asynchronous operations with specific arguments.
    /// </summary>
    /// <typeparam name="TArg">The type representing the required argument for the action.</typeparam>
    /// <typeparam name="TUpdate">The type representing the update associated with the action,
    /// and it must implement the <see cref="ICastedUpdate"/> interface.</typeparam>
    /// <param name="args">The arguments to work with during the action.</param>
    /// <param name="update">The incoming, casted update associated with the action.</param>
    /// <returns>A task representing the asynchronous operation of the action with specific arguments.</returns>
    public delegate Task BotArgedInteraction<TArg, TUpdate>(TArg args, TUpdate update) where TUpdate : ICastedUpdate;
}