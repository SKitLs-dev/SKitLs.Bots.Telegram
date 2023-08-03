using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using System.Reflection;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Partial
{
    /// <summary>
    /// <see cref="PartialSubProcess{TResult}"/> is a special class of input behavior.
    /// Implements an abstract <see cref="TextInputsProcessBase{TResult}"/>.
    /// Used as <see cref="PartialInputProcess{TResult}"/> interior sub-process.
    /// <para>
    /// By its methods similar to
    /// Shot input help to realize input-and-forget process for simple objects that could be unpack via defined
    /// <see cref="IArgsSerializeService.Unpack{TOut}(string)"/>.</para>
    /// <para>
    /// In that case "someId" input will be converted to some object <c>SomeType object = new(id: "someId")</c>
    /// if appropriate <see cref="ConvertRule{TOut}"/> for yours SomeType exists.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type of the wrapped argument.</typeparam>
    public class PartialSubProcess<TResult> : ISubProcess<PartialInputRunning<TResult>> where TResult : notnull
    {
        /// <summary>
        /// Represents the order of the sub-process within the parent bot running process.
        /// </summary>
        public int SubOrder { get; internal set; }
        /// <summary>
        /// Determines whether this sub-process is terminational.
        /// </summary>
        public bool IsTerminational => false;

        /// <summary>
        /// Represents the handling property of the sub-process.
        /// </summary>
        public PropertyInfo HandlingProperty { get; set; }
        /// <summary>
        /// Represents the startup message of the sub-process.
        /// </summary>
        public IDynamicMessage StartupMessage { get; set; }
        /// <summary>
        /// Represents the input preview delegate for the sub-process.
        /// </summary>
        public InputPreviewDelegate<IBuildableMessage>? InputPreview { get; set; }
        /// <summary>
        /// Represents the parse input delegate for the sub-process.
        /// </summary>
        public ParseInputDelegate ParseInput { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialSubProcess{TResult}"/> class with the specified property, startup message, and parser.
        /// </summary>
        /// <param name="property">The handling property of the sub-process.</param>
        /// <param name="startupMessage">The startup message of the sub-process.</param>
        /// <param name="parser">The parse input delegate for the sub-process.</param>
        public PartialSubProcess(PropertyInfo property, IDynamicMessage startupMessage, ParseInputDelegate? parser)
        {
            if (!typeof(TResult).GetProperties().Contains(property)) throw new ArgumentException(nameof(property));
            HandlingProperty = property;
            StartupMessage = startupMessage;
            ParseInput = parser ?? (u => u.Text);
        }

        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="owner">Parent bot running process, that has raised running.</param>
        /// <returns>The running bot process instance representing the ongoing execution of the process.</returns>
        public ISubRunning<PartialInputRunning<TResult>> GetRunning(PartialInputRunning<TResult> owner) => new PartialSubRunning<TResult>(owner, this);

        /// <summary>
        /// Returns a string that represents current object.
        /// </summary>
        /// <returns>A string that represents current object.</returns>
        public override string ToString() => $"Partial Sub for {HandlingProperty.DeclaringType?.Name}.{HandlingProperty.Name}";
    }
}