using SKitLs.Bots.Telegram.Core.Prototypes;
using System.Collections;

namespace SKitLs.Bots.Telegram.Stateful.Model
{
    public class DefaultStateAnswer : IReadOnlyCollection<UserStateDesc>
    {
        internal List<UserStateDesc> SuitableStates { get; set; }
        internal IOutputMessage Message { get; set; }

        public int Count => throw new NotImplementedException();

        public DefaultStateAnswer(List<UserStateDesc> suitStates, IOutputMessage message)
        {
            SuitableStates = suitStates.DistinctBy(x => x.StateId).ToList();
            Message = message;
        }
        public DefaultStateAnswer(UserStateDesc state, IOutputMessage message)
            : this(new List<UserStateDesc>() { state }, message) { }
        //public DefaultStateAnswer(UserStateDesc state, string message)
        //    : this(state, new OutputMessageDecorText(message)) { }
        public DefaultStateAnswer(int stateId, IOutputMessage message)
            : this((UserStateDesc)stateId, message) { }
        //public DefaultStateAnswer(int stateId, string message)
        //    : this((UserStateDesc)stateId, message) { }

        public IEnumerator<UserStateDesc> GetEnumerator()
        {
            foreach (UserStateDesc state in SuitableStates)
                yield return state;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
