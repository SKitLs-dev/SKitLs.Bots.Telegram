using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;
using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Shot;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Net.Http.Headers;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Util
{
    public sealed class TerminatorProcess<TResult> : TextInputsProcessBase<TResult> where TResult : notnull
    {
        private const string StartupText = "Terminational process initialized.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processData">The process's main data.</param>
        /// <param name="overByInput">The action that is invoked when the running bot process is completed.</param>
        public TerminatorProcess(IST processData, ProcessCompletedByInput<TResult> overByInput)
            : base(processData, (a, u) => new OutputMessageText(StartupText), overByInput) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="overByInput">The action that is invoked when the running bot process is completed.</param>
        public TerminatorProcess(string processDefId, IUserState processState, string terminationalKey, ProcessCompletedByInput<TResult> overByInput)
            : base(processDefId, processState, terminationalKey, (a, u) => new OutputMessageText(StartupText), overByInput) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processData">The process's main data.</param>
        /// <param name="overByCallback">The action that is invoked when the running bot process is completed.</param>
        public TerminatorProcess(IST processData, ProcessCompletedByCallback<TResult> overByCallback)
            : base(processData, (a, u) => new OutputMessageText(StartupText), overByCallback) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="overByCallback">The action that is invoked when the running bot process is completed.</param>
        public TerminatorProcess(string processDefId, IUserState processState, string terminationalKey, ProcessCompletedByCallback<TResult> overByCallback)
            : base(processDefId, processState, terminationalKey, (a, u) => new OutputMessageText(StartupText), overByCallback) { }

        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the bot process is running.</param>
        /// <param name="args">The specific arguments required to execute the bot process.</param>
        /// <returns><see cref="TerminatorRunning{TResult}"/> instance representing the ongoing execution of the process.</returns>
        public override IBotRunningProcess GetRunning(long userId, TextInputsArguments<TResult> args) => new TerminatorRunning<TResult>(userId, args, this);
    }
}