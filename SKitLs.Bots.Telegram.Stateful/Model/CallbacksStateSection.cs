//using SKitLs.Bots.Telegram.Interactions.Prototype;
//using System.Collections;

//namespace SKitLs.Bots.Telegram.Stateful.Model
//{
//    public sealed class CallbacksStateSection : InteractionStateSection, IEnumerable<IBotCallback>
//    {
//        public CallbacksStateSection(string name) : base(name) => AvailableCallbacks = new();

//        public List<IBotCallback> AvailableCallbacks { get; set; }
//        public bool AddCallbackSafely(IBotCallback callback)
//        {
//            if (AvailableCallbacks.Find(c => c.Base == callback.Base) == null)
//            {
//                AvailableCallbacks.Add(callback);
//                return true;
//            }
//            else return false;
//        }
//        public int AddCallbacksRangeSafly(List<IBotCallback> commands)
//        {
//            int fault = 0;
//            foreach (IBotCallback command in commands)
//                if (!AddCallbackSafely(command))
//                    fault++;
//            return fault;
//        }
//        public IEnumerator<IBotCallback> GetEnumerator()
//        {
//            foreach (IBotCallback callback in AvailableCallbacks)
//                yield return callback;
//        }
//        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

//        public static CallbacksStateSection Hardcode(string name, List<IBotCallback> callbacks)
//            => new(name) { AvailableCallbacks = callbacks };

//        public override string ToString() => Name;
//    }
//}