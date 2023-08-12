using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Numbers
{
    /// <summary>
    /// <see cref="IntInputProcess"/> is a special class of input behavior. Implements an abstract <see cref="TextInputsProcessBase{TResult}"/>.
    /// <para>
    /// Int input help to realize input-and-forget process for <see cref="int"/> objects. They are unpack via defined
    /// <see cref="IArgsSerializeService.Unpack{TOut}(string)"/>.
    /// </para>
    /// </summary>
    public class IntInputProcess : TextInputsProcessBase<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntInputProcess"/> class with the specified parameters.
        /// </summary>
        /// <param name="processData">The process's main data.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByInput">The action that is invoked when the running bot process is completed.</param>
        public IntInputProcess(IST processData, DynamicArg<int> startupMessage, ProcessCompletedByInput<int> overByInput)
            : base(processData, startupMessage, overByInput) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="IntInputProcess"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByInput">The action that is invoked when the running bot process is completed.</param>
        public IntInputProcess(string processDefId, string terminationalKey, IUserState processState, DynamicArg<int> startupMessage, ProcessCompletedByInput<int> overByInput)
            : base(processDefId, processState, terminationalKey, startupMessage, overByInput) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="IntInputProcess"/> class with the specified parameters.
        /// </summary>
        /// <param name="processData">The process's main data.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByCallback">The action that is invoked when the running bot process is completed.</param>
        public IntInputProcess(IST processData, DynamicArg<int> startupMessage, ProcessCompletedByCallback<int> overByCallback)
            : base(processData, startupMessage, overByCallback) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="IntInputProcess"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByCallback">The action that is invoked when the running bot process is completed.</param>
        public IntInputProcess(string processDefId, string terminationalKey, IUserState processState, DynamicArg<int> startupMessage, ProcessCompletedByCallback<int> overByCallback)
            : base(processDefId, processState, terminationalKey, startupMessage, overByCallback) { }

        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the bot process is running.</param>
        /// <param name="args">The specific arguments required to execute the bot process.</param>
        /// <returns><see cref="IntInputRunning"/> instance representing the ongoing execution of the process.</returns>
        public override IBotRunningProcess GetRunning(long userId, TextInputsArguments<int> args) => new IntInputRunning(userId, args, this);
    }
}