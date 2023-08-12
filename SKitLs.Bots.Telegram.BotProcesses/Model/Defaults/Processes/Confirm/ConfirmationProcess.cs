using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Stateful.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Confirm
{
    /// <summary>
    /// Represents a sealed class for a confirmation process, implementing interfaces for bot processes and actions.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the confirmation process.</typeparam>
    public sealed class ConfirmationProcess<TResult> : IBotProcess<TextInputsArguments<TResult>>, IBotAction<SignedCallbackUpdate>, IApplicant<IStatefulActionManager<SignedCallbackUpdate>> where TResult : notnull
    {
        /// <inheritdoc/>
        public string ActionId => ProcessDefId;
        /// <inheritdoc/>
        public string ProcessDefId { get; private set; }
        /// <inheritdoc/>
        public IUserState ProcessState { get; private set; }
        /// <summary>
        /// Represents the specific token that the action's data is split with.
        /// </summary>
        public char SplitToken { get; set; } = ';';

        /// <summary>
        /// Represents the startup message content builder for the process.
        /// </summary>
        public DynamicArg<TResult>? StartupMessage { get; private set; }
        /// <summary>
        /// Represents the delegate to be invoked when a decision is made in the confirmation process.
        /// </summary>
        public ProcessCompletedByCallback<TResult> OnDecision { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The process definition identifier.</param>
        /// <param name="processState">The user state associated with the process.</param>
        /// <param name="onDecision">The delegate to be invoked when a decision is made in the confirmation process.</param>
        /// <param name="startupMessage">The startup message content builder for the process (optional).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="onDecision"/> is null.</exception>
        public ConfirmationProcess(string processDefId, IUserState processState, ProcessCompletedByCallback<TResult> onDecision, DynamicArg<TResult>? startupMessage = null)
        {
            ProcessDefId = processDefId;
            ProcessState = processState;
            OnDecision = onDecision ?? throw new ArgumentNullException(nameof(onDecision));
            StartupMessage = startupMessage;
        }

        /// <summary>
        /// Updates the process definition identifier and user state using the provided IST (Instance-Specific Transformation) data.
        /// </summary>
        /// <param name="processData">The <see cref="IST"/> data containing the process identifier and user state to be updated.</param>
        public void UpdateIST(IST processData)
        {
            ProcessDefId = processData.Id ?? ProcessDefId;
            ProcessState = processData.State ?? ProcessState;
        }

        /// <inheritdoc/>
        public bool ShouldBeExecutedOn(SignedCallbackUpdate update) => true;
        //update.Data.Contains(SplitToken) && ProcessDefId == update.Data[..update.Data.IndexOf(SplitToken)];

        /// <inheritdoc/>
        public string GetSerializedData(params string[] args) => $"{ProcessDefId}{SplitToken}{args.FirstOrDefault()}";
        /// <summary>
        /// Gets the serialized data that represents accepted process.
        /// </summary>
        /// <returns>The serialized data.</returns>
        public string GetYesCallback() => GetSerializedData(true.ToString());
        /// <summary>
        /// Gets the serialized data that represents declined process.
        /// </summary>
        /// <returns>The serialized data.</returns>
        public string GetNoCallback() => GetSerializedData(false.ToString());

        /// <inheritdoc/>
        public BotInteraction<SignedCallbackUpdate> Action => HandleUpdate;
        private async Task HandleUpdate(SignedCallbackUpdate update)
        {
            if (update.Sender is not IStatefulUser stateful)
                throw new NotStatefulException(this);

            var running = update.Owner.ResolveService<IProcessManager>().GetRunning(stateful)
                ?? throw new SKTgException("procs.NoRunning", SKTEOriginType.Inexternal, ProcessDefId, stateful.TelegramId.ToString());
            await running.HandleInput(update);
        }

        /// <inheritdoc/>
        public void ApplyTo(IStatefulActionManager<SignedCallbackUpdate> entity)
        {
            if (ProcessState is null) throw new SKTgException("procs.NoState", SKTEOriginType.External, ProcessDefId);
            var section = new DefaultStateSection<SignedCallbackUpdate>();
            section.EnableState(ProcessState);
            section.AddSafely(this);
            entity.AddSectionSafely(section);
        }

        /// <inheritdoc/>
        public bool Equals(IBotAction<SignedCallbackUpdate>? other)
        {
            if (other is IBotProcess process)
                return process.ProcessDefId.Equals(ProcessDefId);
            else if (other is IBotAction<SignedCallbackUpdate> action)
                return action.ActionId.Equals(ActionId);
            else
                return false;
        }

        /// <inheritdoc/>
        public IBotRunningProcess GetRunning(long userId, TextInputsArguments<TResult> args) => new ConfirmationRunning<TResult>(userId, args, this);
    }
}