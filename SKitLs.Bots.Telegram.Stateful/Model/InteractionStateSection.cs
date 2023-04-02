namespace SKitLs.Bots.Telegram.Stateful.Model
{
    public abstract class InteractionStateSection
    {
        internal List<UserStateDesc> EnabledStates { get; set; }
        internal bool AnyStateEnabled { get; set; }
        public string Name { get; set; }

        public InteractionStateSection(string name)
        {
            EnabledStates = new();
            AnyStateEnabled = false;
            Name = name;
        }

        public bool ShouldBeExecuted(int stateId)
        {
            if (!AnyStateEnabled)
                return EnabledStates.Find(x => x.StateId == stateId) != null;
            else return true;
        }

        public InteractionStateSection EnableAnyState()
        {
            AnyStateEnabled = true;
            return this;
        }
        public InteractionStateSection DisableAnyState()
        {
            AnyStateEnabled = false;
            return this;
        }
        public InteractionStateSection AddEnabledState(UserStateDesc state)
        {
            EnabledStates.Add(state);
            return this;
        }
        public InteractionStateSection AddEnabledState(int stateId)
        {
            EnabledStates.Add(stateId);
            return this;
        }
        public InteractionStateSection AddEnabledStatesRange(int min, int max)
        {
            for (int i = min; i < max; i++)
                EnabledStates.Add(i);
            return this;
        }

        public CommandsStateSection ToComSS() => (CommandsStateSection)this;
        public CallbacksStateSection ToCallSS() => (CallbacksStateSection)this;
        public InputStateSection ToInputSS() => (InputStateSection)this;
    }
}
