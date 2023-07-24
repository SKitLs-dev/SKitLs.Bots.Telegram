using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Interactions.Defaults;
using SKitLs.Bots.Telegram.Core.Model.Management;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Model
{
    public class BotArgedCallback<TArg> : DefaultCallback, IArgedAction<TArg, SignedCallbackUpdate> where TArg : notnull, new()
    {
        public char SplitToken { get; set; }

        public BotArgedInteraction<TArg, SignedCallbackUpdate> ArgAction { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="BotArgedCallback{TArg}"/> with specified data.
        /// </summary>
        /// <param name="data">Data pair, that contains string that identifies action string that appears on menu's button, presenting action.</param>
        /// <param name="action">Executable action which is invoked on trigger.</param>
        /// <param name="splitToken">Specific character used for seperating arguments from each other.</param>
        public BotArgedCallback(LabeledData data, BotArgedInteraction<TArg, SignedCallbackUpdate> action, char splitToken = ';')
            : this(data.Data, data.Label, action, splitToken) { }
        /// <summary>
        /// Creates a new instance of <see cref="BotArgedCallback{TArg}"/> with specified data.
        /// </summary>
        /// <param name="base">String that used to identify this action.</param>
        /// <param name="label">String that appears on menu's button, presenting this action.</param>
        /// <param name="action">Executable action which is invoked on trigger.</param>
        /// <param name="splitToken">Specific character used for seperating arguments from each other.</param>
        public BotArgedCallback(string @base, string label, BotArgedInteraction<TArg, SignedCallbackUpdate> action, char splitToken = ';') : base(@base, label)
        {
            Action = MiddleAction;
            ArgAction = action;
            SplitToken = splitToken;
        }

        /// <summary>
        /// Checks either this action should be executed on a certain incoming update.
        /// </summary>
        /// <param name="update">An incoming update</param>
        /// <returns><see langword="true"/> if this action should be executed; otherwise, <see langword="false"/>.</returns>
        public override bool ShouldBeExecutedOn(SignedCallbackUpdate update) => update.Data.Contains(SplitToken) && ActionNameBase == update.Data[..update.Data.IndexOf(SplitToken)];
        public ConvertResult<TArg> DeserializeArgs(SignedCallbackUpdate update, IArgsSerilalizerService serilalizer)
            => serilalizer.Deserialize<TArg>(update.Data[(update.Data.IndexOf(SplitToken) + 1)..], SplitToken);
        public string SerializeArgs(TArg data, IArgsSerilalizerService serialize) => serialize.Serialize(data, SplitToken);
        /// <summary>
        /// Generates ready-to-use <see cref="string"/>, consisted of <see cref="ActionNameBase"/> and serialized arguments,
        /// that can be used as actor.
        /// </summary>
        /// <param name="data">An agrument object that should be serialized.</param>
        /// <param name="serialize">Bot's serilization service.</param>
        /// <returns>Ready-to-use <see cref="string"/> that can be used as actor.</returns>
        public string GetSerializedData(TArg data, IArgsSerilalizerService serialize) => ActionNameBase + SerializeArgs(data, serialize);

        /// <summary>
        /// Middle action that is raised by <see cref="IActionManager{TUpdate}"/>.
        /// Used to deserialize arguments, came with update, and pass them to <see cref="ArgAction"/>.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        private async Task MiddleAction(SignedCallbackUpdate update)
        {
            var argService = update.Owner.ResolveService<IArgsSerilalizerService>();
            var args = DeserializeArgs(update, argService).Value;
            await ArgAction.Invoke(args, update);
        }
    }
}