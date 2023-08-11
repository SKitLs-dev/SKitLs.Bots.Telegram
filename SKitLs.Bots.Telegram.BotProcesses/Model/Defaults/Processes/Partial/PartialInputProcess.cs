using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Partial
{
    /// <summary>
    /// <see cref="PartialInputProcess{TResult}"/> is a special class of input behavior.
    /// Implements an abstract <see cref="TextInputsProcessBase{TResult}"/>.
    /// <para>
    /// Partial input help to realize step-by-step input process for complex objects with several properties.
    /// All of them should be unpack via defined <see cref="IArgsSerializeService.Unpack{TOut}(string)"/>.
    /// </para>
    /// <para>
    /// In that case all necessary data will be asked step-by-step until everything is completed properly.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    public class PartialInputProcess<TResult> : TextInputsProcessBase<TResult> where TResult : notnull
    {
        private readonly List<PartialSubProcess<TResult>> subProcesses = new();
        /// <summary>
        /// Gets <see cref="IReadOnlyList{T}"/> that represents internal sub-processes storage.
        /// </summary>
        public IReadOnlyList<PartialSubProcess<TResult>> SubProcesses => subProcesses.OrderBy(x => x.SubOrder).ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processData">The process's main data.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByInput">The action that is invoked when the running bot process is completed.</param>
        public PartialInputProcess(IST processData, DynamicArg<TResult> startupMessage, ProcessCompletedByInput<TResult> overByInput)
            : base(processData, startupMessage, overByInput) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PartialInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByInput">The action that is invoked when the running bot process is completed.</param>
        public PartialInputProcess(string processDefId, string terminationalKey, IUserState processState, DynamicArg<TResult> startupMessage, ProcessCompletedByInput<TResult> overByInput)
            : base(processDefId, processState, terminationalKey, startupMessage, overByInput) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PartialInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processData">The process's main data.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByCallback">The action that is invoked when the running bot process is completed.</param>
        public PartialInputProcess(IST processData, DynamicArg<TResult> startupMessage, ProcessCompletedByCallback<TResult> overByCallback)
            : base(processData, startupMessage, overByCallback) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PartialInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByCallback">The action that is invoked when the running bot process is completed.</param>
        public PartialInputProcess(string processDefId, string terminationalKey, IUserState processState, DynamicArg<TResult> startupMessage, ProcessCompletedByCallback<TResult> overByCallback)
            : base(processDefId, processState, terminationalKey, startupMessage, overByCallback) { }

        /// <summary>
        /// Adds sub-process to internal storage.
        /// </summary>
        /// <param name="subProcess">An item to add.</param>
        public void AddSub(PartialSubProcess<TResult> subProcess)
        {
            subProcess.SubOrder = subProcesses.Count;
            subProcesses.Add(subProcess);
        }
        /// <summary>
        /// Adds range of sub-processes to internal storage..
        /// </summary>
        /// <param name="subProcesses">Items range to add.</param>
        public void AddSubRange(IEnumerable<PartialSubProcess<TResult>> subProcesses)
        {
            foreach (var sub in subProcesses)
                AddSub(sub);
        }

        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the bot process is running.</param>
        /// <param name="args">The specific arguments required to execute the bot process.</param>
        /// <returns><see cref="PartialInputRunning{TResult}"/> instance representing the ongoing execution of the process.</returns>
        public override IBotRunningProcess GetRunning(long userId, TextInputsArguments<TResult> args) => new PartialInputRunning<TResult>(userId, args, this);
    }
}