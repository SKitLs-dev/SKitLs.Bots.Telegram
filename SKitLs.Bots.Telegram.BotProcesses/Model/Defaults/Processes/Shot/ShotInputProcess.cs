using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Stateful.Prototype;
using System.Text.RegularExpressions;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Shot
{
    /// <summary>
    /// <see cref="ShotInputProcess{TResult}"/> is a special class of input behavior.
    /// Implements an abstract <see cref="TextInputsProcessBase{TResult}"/>.
    /// <para> Shot input help to realize input-and-forget process for simple objects that could be unpack via defined
    /// <see cref="IArgsSerializeService.Unpack{TOut}(string)"/>.</para>
    /// <para>
    /// In that case "someId" input will be converted to some object <c>SomeType object = new(id: "someId")</c>
    /// if appropriate <see cref="ConvertRule{TOut}"/> for yours SomeType exists.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable.</typeparam>
    public class ShotInputProcess<TResult> : TextInputsProcessBase<TResult>, IMaskedInput where TResult : notnull
    {
        /// <summary>
        /// Determines special string mask that would be used to unpack input text.
        /// </summary>
        public string Mask { get; set; } = "{0}";

        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processData">The process's main data.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByInput">The action that is invoked when the running bot process is completed.</param>
        public ShotInputProcess(IST processData, DynamicArg<TResult> startupMessage, ProcessCompletedByInput<TResult> overByInput)
            : base(processData, startupMessage, overByInput) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByInput">The action that is invoked when the running bot process is completed.</param>
        public ShotInputProcess(string processDefId, string terminationalKey, IUserState processState, DynamicArg<TResult> startupMessage, ProcessCompletedByInput<TResult> overByInput)
            : base(processDefId, processState, terminationalKey, startupMessage, overByInput) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processData">The process's main data.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByCallback">The action that is invoked when the running bot process is completed.</param>
        public ShotInputProcess(IST processData, DynamicArg<TResult> startupMessage, ProcessCompletedByCallback<TResult> overByCallback)
            : base(processData, startupMessage, overByCallback) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="overByCallback">The action that is invoked when the running bot process is completed.</param>
        public ShotInputProcess(string processDefId, string terminationalKey, IUserState processState, DynamicArg<TResult> startupMessage, ProcessCompletedByCallback<TResult> overByCallback)
            : base(processDefId, processState, terminationalKey, startupMessage, overByCallback) { }

        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the bot process is running.</param>
        /// <param name="args">The specific arguments required to execute the bot process.</param>
        /// <returns><see cref="ShotInputRunning{TResult}"/> instance representing the ongoing execution of the process.</returns>
        public override IBotRunningProcess GetRunning(long userId, TextInputsArguments<TResult> args) => new ShotInputRunning<TResult>(userId, args, this);
    }
}