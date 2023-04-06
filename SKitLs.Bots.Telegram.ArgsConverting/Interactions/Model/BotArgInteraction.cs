//using SKitLs.Bots.Telegram.Core.Prototypes;

//namespace SKitLs.Bots.Telegram.ArgsInteraction.Interactions.Model
//{
//    public abstract class BotArgInteraction : InteractionArgBase
//    {
//        public string Base { get; set; }

//        public int RequiredPermissionLevel { get; set; }
//        public IOutputMessage? NotEnoughRightsMessage { get; set; }

//        public BotArgInteraction(string @base, int permissionLevel, IOutputMessage? notEnoughRightsMes)
//            : base(@base)
//        {
//            Base = @base;
//            RequiredPermissionLevel = permissionLevel;
//            NotEnoughRightsMessage = notEnoughRightsMes;
//        }

//        public BotArgInteraction ExtendBase(string data)
//        {
//            Base += data;
//            return this;
//        }

//        public bool ShouldBeExecutedOn(string message)
//            => HasArgs
//            ? message.ToLower().StartsWith(Base.ToLower())
//            : message.ToLower() == Base.ToLower();
//    }
//}