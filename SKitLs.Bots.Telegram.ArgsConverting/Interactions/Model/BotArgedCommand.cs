using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Management;
using SKitLs.Bots.Telegram.Core.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    /// <summary>
    /// Represents a text input action with argument support, extending the functionality of <see cref="DefaultCommand"/>.
    /// Implements the <see cref="IArgedAction{TArg, TUpdate}"/> interface for the <see cref="SignedMessageTextUpdate"/>
    /// where <typeparamref name="TArg"/> represents the type of required arguments.
    /// <para/>
    /// <b>Example:</b>
    /// <para/>
    /// For the input data <c>'/upvote 37'</c> with the default split token <c>' '</c> and an <see cref="int"/> argument,
    /// '37' would be parsed as an argument.
    /// </summary>
    /// <typeparam name="TArg">The type representing the necessary arguments for the action.
    /// It must not be null and should support a parameterless constructor.</typeparam>
    public class BotArgedCommand<TArg> : DefaultCommand, IArgedAction<TArg, SignedMessageTextUpdate> where TArg : notnull, new()
    {
        /// <summary>
        /// <inheritdoc/>
        /// <para/>
        /// The default split token is <c>' '</c>. For the data <c>'/upvote 37'</c>, '37' would be parsed as an argument.
        /// </summary>
        public char SplitToken { get; set; }

        /// <inheritdoc/>
        public BotArgedInteraction<TArg, SignedMessageTextUpdate> ArgAction { get; protected set; }

        /// <summary>
        /// Creates a new instance of the <see cref="BotArgedCommand{TArg}"/> class with specified data.
        /// </summary>
        /// <param name="base">The string used to identify this action.</param>
        /// <param name="action">The executable action that is invoked on trigger.</param>
        /// <param name="splitToken">The specific character used for separating arguments from each other.</param>
        public BotArgedCommand(string @base, BotArgedInteraction<TArg, SignedMessageTextUpdate> action, char splitToken = ' ') : base(@base, null!)
        {
            Action = MiddleAction;
            ArgAction = action;
            SplitToken = splitToken;
        }

        /// <inheritdoc/>
        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => update.Text.Contains(SplitToken) && $"/{ActionNameBase}" == update.Text[..update.Text.IndexOf(SplitToken)];

        /// <inheritdoc/>
        public ConvertResult<TArg> DeserializeArgs(SignedMessageTextUpdate update, IArgsSerializeService serializer)
            => serializer.Deserialize<TArg>(update.Text[(update.Text.IndexOf(SplitToken) + 1)..], SplitToken);

        /// <inheritdoc/>
        public string SerializeArgs(TArg data, IArgsSerializeService serializer) => serializer.Serialize(data, SplitToken);

        /// <inheritdoc/>
        public string GetSerializedData(TArg data, IArgsSerializeService serializer) => ActionNameBase + SerializeArgs(data, serializer);

        /// <summary>
        /// The middle action that is triggered by the <see cref="IActionManager{TUpdate}"/>.
        /// It is used to deserialize arguments from the update and pass them to the <see cref="ArgAction"/>.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        private async Task MiddleAction(SignedMessageTextUpdate update)
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