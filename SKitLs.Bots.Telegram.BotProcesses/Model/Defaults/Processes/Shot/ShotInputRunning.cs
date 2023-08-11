using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Shot
{
    /// <summary>
    /// The running version of the <see cref="ShotInputProcess{TResult}"/>. See it for info.
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    public class ShotInputRunning<TResult> : TextInputsRunningBase<ShotInputProcess<TResult>, TResult> where TResult : notnull
    {
        /// <summary>
        /// Represents the process arguments associated with the running bot process.
        /// </summary>
        public override TextInputsArguments<TResult> Arguments { get; protected set; }
        /// <summary>
        /// Represents the bot process definition that launched this running process.
        /// </summary>
        public override ShotInputProcess<TResult> Launcher { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputRunning{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns and initiated the running bot process.</param>
        /// <param name="args">The process arguments associated with the running bot process.</param>
        /// <param name="launcher">The bot process definition that launched this running process.</param>
        public ShotInputRunning(long userId, TextInputsArguments<TResult> args, ShotInputProcess<TResult> launcher) : base(userId)
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
            await base.HandleInput(update);
            if (Arguments.CompleteStatus == ProcessCompleteStatus.Pending)
            {
                var input = Launcher is IMaskedInput masked ? masked.Demask(update.Text) : update.Text;
                var result = update.Owner.ResolveService<IArgsSerializeService>()
                    .Unpack<TResult>(input);
                
                await HandleConversionAsync(result, update);
            }
            await TerminateAsync(update);
        }
    }
}