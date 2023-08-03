using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Partial
{
    /// <summary>
    /// The running version of the <see cref="PartialInputRunning{TResult}"/>. See it for info.
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    public class PartialInputRunning<TResult> : TextInputsRunningBase<PartialInputProcess<TResult>, TResult> where TResult : notnull
    {
        /// <summary>
        /// Represents the process arguments associated with the running bot process.
        /// </summary>
        public override TextInputsArguments<TResult> Arguments { get; protected set; }
        /// <summary>
        /// Represents the bot process definition that launched this running process.
        /// </summary>
        public override PartialInputProcess<TResult> Launcher { get; protected set; }

        /// <summary>
        /// Gets <see cref="IReadOnlyList{T}"/> that represents internal sub-processes storage.
        /// </summary>
        public IReadOnlyList<PartialSubProcess<TResult>> SubProcesses => Launcher.SubProcesses;
        private int CurrentId { get; set; }
        private PartialSubRunning<TResult> Current => (PartialSubRunning<TResult>)SubProcesses[CurrentId].GetRunning(this);

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialInputRunning{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns and initiated the running bot process.</param>
        /// <param name="args">The process arguments associated with the running bot process.</param>
        /// <param name="launcher">The bot process definition that launched this running process.</param>
        public PartialInputRunning(long userId, TextInputsArguments<TResult> args, PartialInputProcess<TResult> launcher) : base(userId)
        {
            Arguments = args;
            Launcher = launcher;
        }

        /// <summary>
        /// Handles the input update of type <see cref="SignedMessageTextUpdate"/> for the running bot process.
        /// </summary>
        /// <param name="update">The update containing the input for the bot process.</param>
        public override async Task HandleInput(SignedMessageTextUpdate update)
        {
            if (update.Text.ToLower() == TerminationalKey.ToLower())
            {
                Arguments.CompleteStatus = ProcessCompleteStatus.Canceled;
                await TerminateWithAsync(Arguments, update);
            }
            else
            {
                await Current.HandleInput(update);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        internal async Task Valid(SignedMessageTextUpdate update)
        {
            CurrentId++;
            if (CurrentId < SubProcesses.Count)
            {
                var mes = Current.StartupMessage.BuildWith(update);
                await update.Owner.DeliveryService.ReplyToSender(mes, update);
            }
            else
            {
                Arguments.CompleteStatus = ProcessCompleteStatus.Success;
                await TerminateWithAsync(Arguments, update);
            }
        }
    }
}