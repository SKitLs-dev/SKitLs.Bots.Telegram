using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Numbers
{
    /// <summary>
    /// The running version of the <see cref="IntInputProcess"/>. See it for info.
    /// </summary>
    public class IntInputRunning : TextInputsRunningBase<IntInputProcess, int>
    {
        /// <summary>
        /// Represents the process arguments associated with the running bot process.
        /// </summary>
        public override TextInputsArguments<int> Arguments { get; protected set; }
        /// <summary>
        /// Represents the bot process definition that launched this running process.
        /// </summary>
        public override IntInputProcess Launcher { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntInputRunning"/> class with the specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns and initiated the running bot process.</param>
        /// <param name="args">The process arguments associated with the running bot process.</param>
        /// <param name="launcher">The bot process definition that launched this running process.</param>
        public IntInputRunning(long userId, TextInputsArguments<int> args, IntInputProcess launcher) : base(userId)
        {
            Arguments = args;
            Launcher = launcher ?? throw new ArgumentNullException(nameof(launcher));
        }

        /// <summary>
        /// Handles the input update of type <see cref="SignedMessageTextUpdate"/> for the running bot process.
        /// </summary>
        /// <param name="update">The update containing the input for the bot process.</param>
        public override async Task HandleInput(SignedMessageTextUpdate update)
        {
            Arguments.CompleteStatus = update.Text.ToLower() == TerminationalKey.ToLower()
                ? ProcessCompleteStatus.Canceled
                : ProcessCompleteStatus.Pending;

            var unpack = update.Owner.ResolveService<IArgsSerializeService>().Unpack<int>(update.Text);
            Arguments.CompleteStatus = unpack.ResultType == ConvertResultType.Ok
                ? ProcessCompleteStatus.Success
                : ProcessCompleteStatus.Failure;

            Arguments.BuildingInstance = unpack.Value;

            await TerminateWithAsync(Arguments, update);
        }
    }
}