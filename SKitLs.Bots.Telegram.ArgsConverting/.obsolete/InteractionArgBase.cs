//using SKitLs.Bots.Telegram.ArgsInteraction.Argumenting;
//using System.Reflection;

//namespace SKitLs.Bots.Telegram.ArgsInteraction.Interactions.Model
//{
//    public class InteractionArgBase
//    {
//        public string? ArgsBase { get; set; }

//        internal InteractionArgBase(string? @base)
//        {
//            ArgsType = new();
//            ArgsBase = @base;
//        }

//        public List<Type> ArgsType { get; set; }
//        public bool HasArgs => ArgsType.Count > 0;
//        public char? ArgsSplitter { get; set; }

//        public ConvertResultList ParseArgs(string input)
//        {
//            List<ConvertResult> convertedArgs = new();

//            string data = string.IsNullOrEmpty(ArgsBase) ? input : input[ArgsBase.Length..];

//            string[] args = ArgsSplitter == null
//                ? new[] { data }
//                : data.Trim().Split(ArgsSplitter.Value);

//            if (args.Length != ArgsType.Count)
//                return ConvertResultList.IncorrectRange();
//            for (int i = 0; i < args.Length; i++)
//            {
//                Type convertType = ArgsType[i];
//                MethodInfo? converter = typeof(ArgsExtractorService).GetMethod(nameof(ArgsExtractorService.Instance.Extract));
//                if (converter != null)
//                {
//                    MethodInfo genericConverter = converter.MakeGenericMethod(convertType);
//                    ConvertResult res = (ConvertResult)genericConverter
//                        .Invoke(ArgsExtractorService.Instance, new[] { args[i] })!;
//                    convertedArgs.Add(res);
//                }
//                else
//                    throw new ApplicationException("Unable to resolve method. Feedback to developer.");
//            }

//            return ConvertResultList.WithData(convertedArgs);
//        }
//    }
//}
