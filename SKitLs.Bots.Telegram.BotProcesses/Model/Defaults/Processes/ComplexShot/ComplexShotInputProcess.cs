using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.ComplexShot
{
    /// <summary>
    /// <see cref="ComplexShotInputProcess{TResult}"/> is a special class of input behavior.
    /// Implements an abstract <see cref="TextInputsProcessBase{TResult}"/>.
    /// <para>
    /// Complex shot input help to realize input-and-forget process for complex objects that consists of several properties
    /// and could be deserializes defined <see cref="IArgsSerializeService.Deserialize{TOut}(string, char)"/>.
    /// Appropriate properties should be marked with <see cref="BotActionArgumentAttribute"/>.
    /// </para>
    /// <para>
    /// In that case "Some text\n15\nYes" input will be converted to some object
    /// <c>SomeType(string label = "Some text", int count = 15, bool valid = true)</c>
    /// if all appropriate <see cref="ConvertRule{TOut}"/> for SomeType's interior exist.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument, which must not be nullable and have a parameterless constructor.</typeparam>
    public class ComplexShotInputProcess<TResult> : TextInputsProcessBase<TResult> where TResult : notnull, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComplexShotInputProcess{TResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="whenOver">The action that is invoked when the running bot process is completed.</param>
        public ComplexShotInputProcess(string processDefId, string terminationalKey, IUserState processState, IOutputMessage startupMessage, InputProcessCompleted<TextInputsArguments<TResult>> whenOver)
            : base(processDefId, terminationalKey, processState, startupMessage, whenOver) { }

        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the bot process is running.</param>
        /// <param name="args">The specific arguments required to execute the bot process.</param>
        /// <returns><see cref="ComplexShotInputRunning{TResult}"/> instance representing the ongoing execution of the process.</returns>
        public override IBotRunningProcess GetRunning(long userId, TextInputsArguments<TResult> args) => new ComplexShotInputRunning<TResult>(userId, args, this);
    }
}