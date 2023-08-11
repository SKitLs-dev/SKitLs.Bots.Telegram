using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Stateful.Exceptions.External;
using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Confirm
{
    public sealed class ConfirmationProcess<TResult> : IBotProcess<TextInputsArguments<TResult>>, IBotAction<SignedCallbackUpdate>, IApplicant<IStatefulActionManager<SignedCallbackUpdate>> where TResult : notnull
    {
        public string ActionId => ProcessDefId;
        public string ProcessDefId { get; private set; }
        public IUserState ProcessState { get; private set; }
        // TODO: Maybe exclude.
        public char SplitToken { get; set; } = ';';

        public DynamicArg<TResult>? StartupMessage { get; private set; }
        public ProcessCompletedByCallback<TResult> OnDecision { get; private set; }

        public ConfirmationProcess(string processDefId, IUserState processState, ProcessCompletedByCallback<TResult> onDecision, DynamicArg<TResult>? startupMessage = null)
        {
            ProcessDefId = processDefId;
            ProcessState = processState;
            OnDecision = onDecision ?? throw new ArgumentNullException(nameof(onDecision));
            StartupMessage = startupMessage;
        }

        public void UpdateIST(IST processData)
        {
            ProcessDefId = processData.Id ?? ProcessDefId;
            ProcessState = processData.State ?? ProcessState;
        }

        /// <summary>
        /// Checks either this action should be executed on a certain incoming update.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns><see langword="true"/> if this action should be executed; otherwise, <see langword="false"/>.</returns>
        public bool ShouldBeExecutedOn(SignedCallbackUpdate update) => true;
        //update.Data.Contains(SplitToken) && ProcessDefId == update.Data[..update.Data.IndexOf(SplitToken)];

        public string GetSerializedData(params string[] args) => $"{ProcessDefId}{SplitToken}{args.FirstOrDefault()}";
        public string GetYesCallback() => GetSerializedData(true.ToString());
        public string GetNoCallback() => GetSerializedData(false.ToString());

        public BotInteraction<SignedCallbackUpdate> Action => HandleUpdate;
        private async Task HandleUpdate(SignedCallbackUpdate update)
        {
            if (update.Sender is not IStatefulUser stateful)
                throw new NotStatefulException(this);

            var running = update.Owner.ResolveService<IProcessManager>().GetRunning(stateful)
                ?? throw new SKTgException("procs.NoRunning", SKTEOriginType.Inexternal, ProcessDefId, stateful.TelegramId.ToString());
            await running.HandleInput(update);
        }

        /// <summary>
        /// Applies <see cref="TextInputsProcessBase"/> for an <paramref name="entity"/>, defining itself to its interior:
        /// creating <see cref="IStateSection{TUpdate}"/> with only <see cref="ProcessState"/> enabled.
        /// </summary>
        /// <param name="entity">An instance that this class should be applied to.</param>
        public void ApplyTo(IStatefulActionManager<SignedCallbackUpdate> entity)
        {
            if (ProcessState is null) throw new SKTgException("procs.NoState", SKTEOriginType.External, ProcessDefId);
            var section = new DefaultStateSection<SignedCallbackUpdate>();
            section.EnableState(ProcessState);
            section.AddSafely(this);
            entity.AddSectionSafely(section);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <see langword="false"/>.</returns>
        public bool Equals(IBotAction<SignedCallbackUpdate>? other)
        {
            if (other is IBotProcess process)
                return process.ProcessDefId.Equals(ProcessDefId);
            else if (other is IBotAction<SignedCallbackUpdate> action)
                return action.ActionId.Equals(ActionId);
            else
                return false;
        }

        public IBotRunningProcess GetRunning(long userId, TextInputsArguments<TResult> args) => new ConfirmationRunning<TResult>(userId, args, this);
    }
}