using SKitLs.Bots.Telegram.BotProcesses.Prototype;
using SKitLs.Bots.Telegram.BotProcesses.Prototype.Processes;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
using SKitLs.Bots.Telegram.Stateful.Model;
using SKitLs.Bots.Telegram.Stateful.Prototype;

namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults
{
    /// <summary>
    /// Класс-прослойка между <see cref="IBotProcess"/> и процессами, поддерживающими <see cref="SignedMessageTextUpdate"/>
    /// </summary>
    public abstract class DefaultProcessBase : IBotProcess, IStatefulIntegratable<SignedMessageTextUpdate>, IBotAction<SignedMessageTextUpdate>
    {
        public virtual string ActionId => ProcessDefId;
        public virtual string ProcessDefId { get; protected set; }
        public virtual string TerminationalKey { get; protected set; }
        public virtual IUserState ProcessState { get; protected set; }

        public DefaultProcessBase(string processDefId, string terminationalKey, IUserState processState)
        {
            ProcessDefId = processDefId;
            TerminationalKey = terminationalKey;
            ProcessState = processState;
        }

        public string GetSerializedData(params string[] args) => throw new NotImplementedException();
        /// <summary>
        /// Due to the facts: <see cref="DefaultProcessBase"/> represents mechanics of text input handling
        /// and mechanics of the class are based on <see cref="IUserState"/> - this method will always
        /// return <c>true</c> value.
        /// </summary>
        /// <param name="update">Inherited parameter. <c>Do not influence</c> the result.</param>
        /// <returns>True.</returns>
        public bool ShouldBeExecutedOn(SignedMessageTextUpdate update) => true;
        // TODO
        public bool Equals(IBotAction<SignedMessageTextUpdate>? other) => false;
        public BotInteraction<SignedMessageTextUpdate> Action => HandleInput;

        // TODO
        /// <summary>
        /// Прослойка, как в ArgedAction: чекает в процесс менеджере, есть ли бегущей процесс для этого прототипа
        /// и передаёт ему обработку сообщения.
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task HandleInput(SignedMessageTextUpdate update)
        {
            if (update.Sender is not IStatefulUser stateful)
                throw new Exception("state");
            if (update.Owner.ResolveService<IProcessManager>()
                .GetRunning(stateful) is not IBotRunningProcess running)
                throw new Exception("run");
            await running.HandleInput(update);
        }

        public ICollection<IStateSection<SignedMessageTextUpdate>> GetSectionsList()
        {
            if (ProcessState is null) throw new ArgumentNullException();
            var section = new DefaultStateSection<SignedMessageTextUpdate>();
            section.EnableState(ProcessState);
            section.AddSafely(this);
            return new List<IStateSection<SignedMessageTextUpdate>>() { section };
        }

        public override string ToString() => $"[{GetType().Name}] {ActionId}";
    }
}