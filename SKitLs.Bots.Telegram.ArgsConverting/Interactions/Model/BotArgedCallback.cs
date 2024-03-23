using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    /// <summary>
    /// Represents a callback action with argument support, extending the functionality of <see cref="DefaultCallback"/>.
    /// Implements the <see cref="IArgedAction{TArg, TUpdate}"/> interface for the <see cref="SignedCallbackUpdate"/>
    /// where <typeparamref name="TArg"/> represents the type of required arguments.
    /// <para/>
    /// <b>Example:</b>
    /// <para/>
    /// For the input data <c>'open;13'</c> with the default split token <c>';'</c> and an <see cref="int"/> argument,
    /// '13' would be parsed as an argument.
    /// </summary>
    /// <typeparam name="TArg">The type representing the necessary arguments for the action.
    /// It must not be null and should support a parameterless constructor.</typeparam>
    public class BotArgedCallback<TArg> : DefaultCallback, IArgedAction<TArg, SignedCallbackUpdate> where TArg : notnull, new()
    {
        /// <summary>
        /// <inheritdoc/>
        /// <para/>
        /// The default split token is <c>';'</c>. For the data <c>'open;13'</c>, '13' would be parsed as an argument.
        /// </summary>
        public char SplitToken { get; set; }

        /// <inheritdoc/>
        public BotArgedInteraction<TArg, SignedCallbackUpdate> ArgAction { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="BotArgedCallback{TArg}"/> class with specified data.
        /// </summary>
        /// <param name="data">The action's labeled data pair.</param>
        /// <param name="action">The executable action that is invoked on trigger.</param>
        /// <param name="splitToken">The specific character used for separating arguments from each other.</param>
        public BotArgedCallback(LabeledData data, BotArgedInteraction<TArg, SignedCallbackUpdate> action, char splitToken = ';')
            : this(data.Data, data.Label, action, splitToken) { }

        /// <summary>
        /// Creates a new instance of the <see cref="BotArgedCallback{TArg}"/> class with specified data.
        /// </summary>
        /// <param name="base">The string used to identify this action.</param>
        /// <param name="label">The string that appears on menu's button, presenting this action.</param>
        /// <param name="action">The executable action that is invoked on trigger.</param>
        /// <param name="splitToken">The specific character used for separating arguments from each other.</param>
        public BotArgedCallback(string @base, string label, BotArgedInteraction<TArg, SignedCallbackUpdate> action, char splitToken = ';') : base(@base, label, null!)
        {
            Action = MiddleAction;
            ArgAction = action;
            SplitToken = splitToken;
        }

        /// <inheritdoc/>
        public override bool ShouldBeExecutedOn(SignedCallbackUpdate update) => update.Data.Contains(SplitToken) && ActionNameBase == update.Data[..update.Data.IndexOf(SplitToken)];

        /// <inheritdoc/>
        public ConvertResult<TArg> DeserializeArgs(SignedCallbackUpdate update, IArgsSerializeService serializer)
            => serializer.Deserialize<TArg>(update.Data[(update.Data.IndexOf(SplitToken) + 1)..], SplitToken);

        /// <inheritdoc/>
        public string SerializeArgs(TArg data, IArgsSerializeService serializer) => serializer.Serialize(data, SplitToken);

        /// <inheritdoc/>
        public string GetSerializedData(TArg data, IArgsSerializeService serializer) => ActionNameBase + SerializeArgs(data, serializer);

        /// <summary>
        /// The middle action that is triggered by the <see cref="IActionManager{TUpdate}"/>.
        /// It is used to deserialize arguments from the update and pass them to the <see cref="ArgAction"/>.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        private async Task MiddleAction(SignedCallbackUpdate update)
        {
            var argService = update.Owner.ResolveService<IArgsSerializeService>();
            var args = DeserializeArgs(update, argService);
            if (args.ResultType == ConvertResultType.Ok)
            {
                await ArgAction.Invoke(args.Value, update);
            }
            else
            {
                throw new ArgedInterException("ArgedActionNullValue", SKTEOriginType.External, this, args.ResultMessage);
            }
        }
    }
}