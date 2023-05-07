//using SKitLs.Bots.Telegram.Interactions.Prototype;
//using System.Collections;

//namespace SKitLs.Bots.Telegram.Stateful.Model
//{
//    public class InputStateSection : InteractionStateSection, IEnumerable
//    {
//        public InputStateSection(string name) : base(name) => AvailableInputHandlers = new();

//        private List<IBotTextInput> AvailableInputHandlers { get; set; }
//        public bool AddInputSafely(IBotTextInput command)
//        {
//            AvailableInputHandlers.Add(command);
//            return true;
//        }
//        public int AddInputRangeSafly(List<IBotTextInput> inputHandlers)
//        {
//            foreach (IBotTextInput inputHandler in inputHandlers)
//                AddInputSafely(inputHandler);
//            return 0;
//        }
//        public IEnumerator GetEnumerator()
//        {
//            foreach (var item in AvailableInputHandlers)
//                yield return item;
//        }
//        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//        public static InputStateSection Hardcode(string name, List<IBotTextInput> commands)
//            => new(name) { AvailableInputHandlers = commands };
//    }
//}
