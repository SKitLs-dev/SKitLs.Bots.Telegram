using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.BotProcesses.Model
{
    /// <summary>
    /// Represents a delegate for handling the completion of a process with specified arguments and update.
    /// </summary>
    /// <typeparam name="TArg">The type of process arguments, implementing <see cref="IProcessArgument"/>.</typeparam>
    /// <typeparam name="TUpdate">The type of the update, implementing <see cref="ISignedUpdate"/>.</typeparam>
    /// <param name="args">The arguments of the process.</param>
    /// <param name="update">The update associated with the process completion.</param>
    public delegate Task ProcessCompleted<TArg, TUpdate>(TArg args, TUpdate update) where TArg : IProcessArgument where TUpdate : ISignedUpdate;

    /// <summary>
    /// Represents a delegate for handling the completion of an input process with specified arguments, triggered by <see cref="SignedMessageTextUpdate"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    /// <param name="args">The arguments of the input process.</param>
    /// <param name="update">The <see cref="SignedMessageTextUpdate"/> associated with the input process completion.</param>
    public delegate Task ProcessCompletedByInput<TResult>(TextInputsArguments<TResult> args, SignedMessageTextUpdate update) where TResult : notnull;

    /// <summary>
    /// Represents a delegate for handling the completion of an input process with specified arguments, triggered by <see cref="SignedCallbackUpdate"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    /// <param name="args">The arguments of the input process.</param>
    /// <param name="update">The <see cref="SignedCallbackUpdate"/> associated with the input process completion.</param>
    public delegate Task ProcessCompletedByCallback<TResult>(TextInputsArguments<TResult> args, SignedCallbackUpdate update) where TResult : notnull;

    /// <summary>
    /// Represents a delegate for previewing input data before assigning it.
    /// </summary>
    /// <typeparam name="IMessage">The type of the message, implementing <see cref="IBuildableMessage"/>.</typeparam>
    /// <param name="update">The <see cref="SignedMessageTextUpdate"/> to preview the message for.</param>
    /// <returns>An <typeparamref name="IMessage"/> with previewed error result or <see langword="null"/> if input is valid.</returns>
    public delegate IMessage? InputPreviewDelegate<IMessage>(SignedMessageTextUpdate update) where IMessage : IBuildableMessage;

    /// <summary>
    /// Represents a delegate for parsing input from a <see cref="SignedMessageTextUpdate"/> into an object.
    /// </summary>
    /// <param name="update">The <see cref="SignedMessageTextUpdate"/> to parse the input from.</param>
    /// <returns>A parsed object.</returns>
    public delegate object ParseInputDelegate(SignedMessageTextUpdate update);
}