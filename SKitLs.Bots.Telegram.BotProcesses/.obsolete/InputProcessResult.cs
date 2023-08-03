//namespace SKitLs.Bots.Telegram.BotProcesses.Model.Defaults.Processes.Partial
//{
//    [Obsolete("Use Defaults Instead")]
//    public enum OverType
//    {
//        Accepted = 0,
//        Denied = 1,
//        Canceled = 2,
//    }

//    [Obsolete("Use Defaults Instead")]
//    public class InputProcessResult<TRes>
//    {
//        public TRes? Result { get; set; }
//        public OverType Status { get; set; }

//        public InputProcessResult(TRes? result = default) => Result = result;

//        public static InputProcessResult<TRes> Accepted(TRes value) => new(value) { Status = OverType.Accepted };
//        public static InputProcessResult<TRes> Denied() => new() { Status = OverType.Denied };
//        public static InputProcessResult<TRes> Canceled() => new() { Status = OverType.Canceled };
//    }
//}