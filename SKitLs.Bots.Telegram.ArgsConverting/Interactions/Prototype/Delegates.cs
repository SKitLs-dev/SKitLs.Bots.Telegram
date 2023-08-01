using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype
{
    /// <summary>
    /// Represents a delegate for argumented actions that supports asynchronous operations.
    /// </summary>
    /// <typeparam name="TArg">The type representing the necessary argument for the action.</typeparam>
    /// <typeparam name="TUpdate">The type representing the update that this action works with,
    /// and it must implement the <see cref="ICastedUpdate"/> interface.</typeparam>
    /// <param name="args">The arguments to work with during the action.</param>
    /// <param name="update">The incoming, casted update associated with the action.</param>
    public delegate Task BotArgedInteraction<TArg, TUpdate>(TArg args, TUpdate update) where TUpdate : ICastedUpdate;
}