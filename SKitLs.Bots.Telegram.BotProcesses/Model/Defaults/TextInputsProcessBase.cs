using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Core.Prototype;
using SKitLs.Bots.Telegram.Stateful.Exceptions.External;
using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults
{
    /// <summary>
    /// Abstract class serving as an intermediary between <see cref="IBotProcess"/> and implemented processes supporting <see cref="SignedMessageTextUpdate"/>.
    /// It is designed to handle bot processes involving text-based interactions and requires implementation by derived classes.
    /// <para>
    /// Inheriting from this class will grant access to both the functionality provided by <see cref="IBotProcess"/>
    /// and <see cref="IBotAction{TUpdate}"/> (for <see cref="SignedMessageTextUpdate"/>) interface,
    /// which supports processes involving text-based updates.
    /// </para>
    /// <para>
    /// This abstract class allows to create specialized bot processes focused on processing text-based updates
    /// or interactions with users. It provides a cohesive foundation for handling and managing text-related bot actions
    /// while adhering to the <see cref="IBotProcess"/> interface.
    /// </para>
    /// </summary>
    public abstract class TextInputsProcessBase<TArg> : IBotProcess<TArg>, IBotAction<SignedMessageTextUpdate>, IApplicant<IStatefulActionManager<SignedMessageTextUpdate>> where TArg : IProcessArgument
    {
        /// <summary>
        /// Returns the unique identifier for the action, which is the same as the process definition Id.
        /// </summary>
        public virtual string ActionId => ProcessDefId;
        /// <summary>
        /// Represents the unique identifier of the bot process definition.
        /// It serves as a distinguishing identifier for the particular bot process within the application.
        /// </summary>
        public virtual string ProcessDefId { get; protected set; }
        /// <summary>
        /// Represents the key used to stop and terminate the bot process.
        /// </summary>
        public virtual string TerminationalKey { get; protected set; }
        /// <summary>
        /// Represents the state associated with the bot process.
        /// Allows to maintain user-specific state information, enabling the resumption of interrupted processes
        /// or tracking progress.
        /// </summary>
        public virtual IUserState ProcessState { get; protected set; }
        /// <summary>
        /// Represents the startup message of the bot process.
        /// </summary>
        public virtual IOutputMessage StartupMessage { get; protected set; }
        /// <summary>
        /// Represents the action that is invoked when the running bot process is completed.
        /// </summary>
        public virtual InputProcessCompleted<TArg> WhenOver { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputsProcessBase{TArg}"/> class with the specified process definition Id,
        /// terminational key, and user state.
        /// </summary>
        /// <param name="processDefId">The unique identifier for the bot process.</param>
        /// <param name="terminationalKey">The key used to stop and terminate the bot process.</param>
        /// <param name="processState">The state associated with the bot process.</param>
        /// <param name="startupMessage">The startup message of the bot process.</param>
        /// <param name="whenOver">The action that is invoked when the running bot process is completed.</param>
        public TextInputsProcessBase(string processDefId, string terminationalKey, IUserState processState, IOutputMessage startupMessage, InputProcessCompleted<TArg> whenOver)
        {
            ProcessDefId = processDefId;
            TerminationalKey = terminationalKey;
            ProcessState = processState;
            StartupMessage = startupMessage;
            WhenOver = whenOver;
        }

        /// <summary>
        /// <c>[Not Supported]</c> Gets serialized data that can be built with certain arguments.
        /// </summary>
        /// <param name="args">Arguments to be used.</param>
        /// <returns>Throws <see cref="NotImplementedException"/>.</returns>
        public string GetSerializedData(params string[] args) => throw new NotImplementedException();
        /// <summary>
        /// Determines if the bot process should be executed on the specified signed message text <paramref name="update"/>.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>Always returns <see langword="true"/> since <see cref="TextInputsProcessBase{TArg}"/> represents mechanics
        /// of text input handling based on <see cref="IUserState"/>.</returns>
        public bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => true;

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true"/> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <see langword="false"/>.</returns>
        public bool Equals(IBotAction<SignedMessageTextUpdate>? other)
        {
            if (other is IBotProcess process)
                return process.ProcessDefId.Equals(ProcessDefId);
            else if (other is IBotAction<SignedMessageTextUpdate> action)
                return action.ActionId.Equals(ActionId);
            else
                return false;
        }
        /// <summary>
        /// Represents bot interaction action, which is the method used to handle input for the bot process.
        /// </summary>
        public BotInteraction<SignedMessageTextUpdate> Action => HandleInput;

        /// <summary>
        /// Private method acting as an intermediary to handle input by checking for a running process in the process manager,
        /// and then passing the update for further processing.
        /// </summary>
        /// <param name="update">The signed text message update to be processed.</param>
        /// <exception cref="NotStatefulException">Thrown if the update's sender is not an <see cref="IStatefulUser"/>.</exception>
        /// <exception cref="SKTgSignedException">Thrown if no running process was found.</exception>
        private async Task HandleInput(SignedMessageTextUpdate update)
        {
            if (update.Sender is not IStatefulUser stateful)
                throw new NotStatefulException(this);

            var running = update.Owner.ResolveService<IProcessManager>().GetRunning(stateful)
                ?? throw new SKTgSignedException("procs.NoRunning", SKTEOriginType.Inexternal, this, ProcessDefId, stateful.TelegramId.ToString());
            await running.HandleInput(update);
        }

        /// <summary>
        /// Creates new running bot process instance based on the specified user and arguments.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the bot process is running.</param>
        /// <param name="args">The specific arguments required to execute the bot process.</param>
        /// <returns>The running bot process instance representing the ongoing execution of the process.</returns>
        public abstract IBotRunningProcess GetRunning(long userId, TArg args);

        /// <summary>
        /// Applies <see cref="TextInputsProcessBase{TArg}"/> for an <paramref name="entity"/>, defining itself to its interior:
        /// creating <see cref="IStateSection{TUpdate}"/> with only <see cref="ProcessState"/> enabled.
        /// </summary>
        /// <param name="entity">An instance that this class should be applied to.</param>
        /// <exception cref="SKTgSignedException">Thrown if <see cref="ProcessState"/> is <see langword="null"/>.</exception>
        public void ApplyTo(IStatefulActionManager<SignedMessageTextUpdate> entity)
        {
            if (ProcessState is null) throw new SKTgSignedException("procs.NoState", SKTEOriginType.External, this, ProcessDefId);
            var section = new DefaultStateSection<SignedMessageTextUpdate>();
            section.EnableState(ProcessState);
            section.AddSafely(this);
            entity.AddSectionSafely(section);
        }

        /// <summary>
        /// Returns a string which represents the object instance.
        /// </summary>
        /// <returns>A string which represents the object instance.</returns>
        public override string ToString() => $"[{GetType().Name}] {ActionId}";
    }
}