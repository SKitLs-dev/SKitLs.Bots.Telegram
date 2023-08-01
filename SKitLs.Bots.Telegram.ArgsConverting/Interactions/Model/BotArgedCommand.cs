using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    /// <summary>
    /// Represents a text input action with argument support, derived from <see cref="DefaultCommand"/>.
    /// Implements the <see cref="IArgedAction{TArg, TUpdate}"/> (<typeparamref name="TArg"/>, <see cref="SignedMessageTextUpdate"/>)
    /// interface where <typeparamref name="TArg"/> represents the type of arguments.
    /// </summary>
    /// <typeparam name="TArg">The type representing the necessary arguments for the action.
    /// Must be notnull and support a parameterless constructor.</typeparam>
    public class BotArgedCommand<TArg> : DefaultCommand, IArgedAction<TArg, SignedMessageTextUpdate> where TArg : notnull, new()
    {
        /// <summary>
        /// Represents specific token that action's data is split with.
        /// <para>
        /// Example: <c>';'</c> token by default for <see cref="BotArgedCallback{TArg}"/> so its data is
        /// <c>'callbackName;arg1;arg2;arg3'</c>.
        /// </para>
        /// </summary>
        public char SplitToken { get; set; }

        /// <summary>
        /// An action that should be executed on update.
        /// </summary>
        public BotArgedInteraction<TArg, SignedMessageTextUpdate> ArgAction { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="BotArgedCommand{TArg}"/> with specified data.
        /// </summary>
        /// <param name="base">String that used to identify this action.</param>
        /// <param name="action">Executable action which is invoked on trigger.</param>
        /// <param name="splitToken">Specific character used for separating arguments from each other.</param>
        public BotArgedCommand(string @base, BotArgedInteraction<TArg, SignedMessageTextUpdate> action, char splitToken = ' ') : base(@base)
        {
            Action = MiddleAction;
            ArgAction = action;
            SplitToken = splitToken;
        }

        /// <summary>
        /// Checks either this action should be executed on a certain incoming update.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns><see langword="true"/> if this action should be executed; otherwise, <see langword="false"/>.</returns>
        public override bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => update.Text.Contains(SplitToken) && ActionNameBase == update.Text[..update.Text.IndexOf(SplitToken)];

        /// <summary>
        /// Deserializes an incoming string data to get a specific <typeparamref name="TArg"/> instance with
        /// unpacked received data.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <param name="serializer">Bot's serialization service.</param>
        /// <returns>Result of converting attempt.</returns>
        public ConvertResult<TArg> DeserializeArgs(SignedMessageTextUpdate update, IArgsSerializeService serializer)
            => serializer.Deserialize<TArg>(update.Text[(update.Text.IndexOf(SplitToken) + 1)..], SplitToken);

        /// <summary>
        /// Serializes <paramref name="data"/>'s properties marked with <see cref="BotActionArgumentAttribute"/>
        /// with rules provided by <paramref name="serializer"/>.
        /// </summary>
        /// <param name="data">An argument object that should be serialized.</param>
        /// <param name="serializer">Bot's serialization service.</param>
        /// <returns>Serialized <paramref name="data"/>.</returns>
        public string SerializeArgs(TArg data, IArgsSerializeService serializer) => serializer.Serialize(data, SplitToken);

        /// <summary>
        /// Generates ready-to-use <see cref="string"/>, consisted of <see cref="DefaultBotAction{TUpdate}.ActionNameBase"/>
        /// and serialized arguments, that can be used as actor.
        /// </summary>
        /// <param name="data">An argument object that should be serialized.</param>
        /// <param name="serializer">Bot's serialization service.</param>
        /// <returns>Ready-to-use <see cref="string"/> that can be used as actor.</returns>
        public string GetSerializedData(TArg data, IArgsSerializeService serializer) => ActionNameBase + SerializeArgs(data, serializer);

        /// <summary>
        /// Middle action that is raised by <see cref="IActionManager{TUpdate}"/>.
        /// Used to deserialize arguments, came with update, and pass them to <see cref="ArgAction"/>.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        private async Task MiddleAction(SignedMessageTextUpdate update)
        {
            var argService = update.Owner.ResolveService<IArgsSerializeService>();
            var args = DeserializeArgs(update, argService).Value;
            await ArgAction.Invoke(args, update);
        }
    }
}