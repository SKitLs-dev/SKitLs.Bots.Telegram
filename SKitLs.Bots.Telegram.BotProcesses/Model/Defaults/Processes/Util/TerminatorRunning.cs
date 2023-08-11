using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Shot;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Util
{
    public class TerminatorRunning<TResult> : TextInputsRunningBase<TerminatorProcess<TResult>, TResult> where TResult : notnull
    {
        /// <summary>
        /// Represents the process arguments associated with the running bot process.
        /// </summary>
        public override TextInputsArguments<TResult> Arguments { get; protected set; }
        /// <summary>
        /// Represents the bot process definition that launched this running process.
        /// </summary>
        public override TerminatorProcess<TResult> Launcher { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputRunning{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns and initiated the running bot process.</param>
        /// <param name="args">The process arguments associated with the running bot process.</param>
        /// <param name="launcher">The bot process definition that launched this running process.</param>
        public TerminatorRunning(long userId, TextInputsArguments<TResult> args, TerminatorProcess<TResult> launcher) : base(userId)
        {
            Arguments = args;
            Launcher = launcher;
        }

        /// <summary>
        /// Handles the input update of type <see cref="SignedMessageTextUpdate"/> for the running bot process.
        /// </summary>
        /// <param name="update">The update containing the input for the bot process.</param>
        public override async Task HandleInput(SignedMessageTextUpdate update) => await TerminateAsync(update);

        // TODO
        /// <summary>
        /// Launches and starts the bot process with the provided update.
        /// </summary>
        /// <typeparam name="TUpdate">The type of trigger update that implements the <see cref="ISignedUpdate"/> interface.</typeparam>
        /// <param name="update">The update to launch the bot process with.</param>
        public override async Task LaunchWith<TUpdate>(TUpdate update) => await TerminateAsync(update);
    }
}