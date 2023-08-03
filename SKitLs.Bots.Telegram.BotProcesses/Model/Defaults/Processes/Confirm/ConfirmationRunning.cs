using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Interactions.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Stateful.Exceptions.External;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Confirm
{
    /// <summary>
    /// The running version of the <see cref="ConfirmationProcess{TResult}"/>. See it for info.
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    public class ConfirmationRunning<TResult> : IBotRunningProcess where TResult : IProcessArgument
    {
        /// <summary>
        /// Represents the bot process definition that launched this running process.
        /// </summary>
        public ConfirmationProcess<TResult> Launcher { get; private set; }
        /// <summary>
        /// Represents the unique identifier of the user who owns and initiated the running bot process.
        /// It allows to identify and associate the bot process with the specific user who triggered its execution.
        /// </summary>
        public long OwnerUserId { get; private set; }
        /// <summary>
        /// Represents the wrapped argument value.
        /// </summary>
        public TResult PendingInstance { get; private set; }

        /// <summary>
        /// Represents the unique identifier of the bot process definition.
        /// It serves as a distinguishing identifier for the particular bot process within the application.
        /// </summary>
        public string ProcessDefId => Launcher.ProcessDefId + $"Running.{OwnerUserId}";
        /// <summary>
        /// Represents the state associated with the bot process.
        /// Allows to maintain user-specific state information, enabling the resumption of interrupted processes
        /// or tracking progress.
        /// </summary>
        public IUserState ProcessState => Launcher.ProcessState;
        
        // TODO: Maybe exclude.
        /// <summary>
        /// Split Token, used to separate data.
        /// </summary>
        public char SplitToken => Launcher.SplitToken;

        /// <summary>
        /// Represents the startup message of the bot process.
        /// </summary>
        public IOutputMessage? StartupMessage => Launcher.StartupMessage;
        /// <summary>
        /// Represents the action that is invoked when the running bot process is completed.
        /// </summary>
        public ProcessCompleted<TResult, SignedCallbackUpdate> OnDecision => Launcher.OnDecision;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationRunning{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns and initiated the running bot process.</param>
        /// <param name="value">The pending value, associated with the running bot process, awaiting for confirmation.</param>
        /// <param name="launcher">The bot process definition that launched this running process.</param>
        public ConfirmationRunning(long userId, TResult value, ConfirmationProcess<TResult> launcher)
        {
            OwnerUserId = userId;
            PendingInstance = value;
            Launcher = launcher ?? throw new ArgumentNullException(nameof(launcher));
        }

        /// <summary>
        /// Launches and starts the bot process with the provided update.
        /// </summary>
        /// <typeparam name="TUpdate">The type of trigger update that implements the <see cref="ISignedUpdate"/> interface.</typeparam>
        /// <param name="update">The update to launch the bot process with.</param>
        public async Task LaunchWith<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
        {
            var mes = (StartupMessage is IDynamicMessage dynamic
                ? dynamic.BuildWith(update)
                : StartupMessage)
                ?? new OutputMessageText(update.Owner.ResolveBotString("procs.display.ConfirmDefaultMessage"));

            await update.Owner.DeliveryService.ReplyToSender(mes, update);
        }

        /// <summary>
        /// Handles the input update of type <see cref="SignedMessageTextUpdate"/> for the running bot process.
        /// </summary>
        /// <param name="update">The update containing the input for the bot process.</param>
        public async Task HandleInput<TUpdate>(TUpdate update) where TUpdate : ISignedUpdate
        {
            if (update is not SignedCallbackUpdate callback)
                throw new UpdateMissMatchException(this, typeof(SignedCallbackUpdate), typeof(TUpdate));
            if (callback.Sender is not IStatefulUser stateful)
                throw new NotStatefulException(this);

            var res = update.Owner.ResolveService<IArgsSerializeService>()
                .Deserialize<bool>(callback.Data[(callback.Data.IndexOf(SplitToken) + 1)..], SplitToken);
            PendingInstance.CompleteStatus = res.ResultType == ConvertResultType.Ok
                ? ProcessCompleteStatus.Pending
                : ProcessCompleteStatus.Failure;
            PendingInstance.CompleteStatus = res.Value
                ? ProcessCompleteStatus.Success
                : ProcessCompleteStatus.Canceled;

            update.Owner.ResolveService<IProcessManager>().Terminate(stateful);
            await OnDecision.Invoke(PendingInstance, callback);
        }
    }
}