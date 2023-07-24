using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.External;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using System.Reflection;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumenting
{
    /// <summary>
    /// Represents default realisation of <see cref="IArgsSerilalizerService"/> and provides methods
    /// to serialize and deserialize method arguments, using Converting System Rules.
    /// <para>
    /// <seealso cref="ConvertRule{TOut}"/>, <seealso cref="ConvertResult{TOut}"/>
    /// </para>
    /// </summary>
    public class DefaultArgsSerilalizerService : IArgsSerilalizerService
    {
        private BotManager? _owner;
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(GetType());
            set => _owner = value;
        }
        public Action<object, BotManager>? OnCompilation => null;

        /// <summary>
        /// Gets or sets the list of <see cref="ConvertRule"/> objects, that holds
        /// converting rules for different types.
        /// </summary>
        private List<ConvertRule> Rules { get; set; }

        public ConvertRule<TOut> ResolveTypeRule<TOut>() where TOut : notnull
            => FindRule<TOut>() ?? throw new NotDefinedException(GetType(), typeof(ConvertRule<TOut>), typeof(TOut).Name);
        public ConvertRule<TOut>? FindRule<TOut>() where TOut : notnull
            => (ConvertRule<TOut>?)Rules.Find(r => r.ResultType == typeof(TOut));
        public bool IsDefined<TOut>() where TOut : notnull => FindRule<TOut>() is not null;

        /// <summary>
        /// Default constructor with preset conversion rules for <see cref="int"/>, <see cref="long"/>,
        /// <see cref="float"/>, <see cref="double"/>, <see cref="bool"/> and <see cref="string"/>
        /// </summary>
        public DefaultArgsSerilalizerService()
        {
            Rules = new()
            {
                new ConvertRule<int>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<int>.NullInput();
                    else if (!int.TryParse(input, out int res))
                        return ConvertResult<int>.Incorrect();
                    else
                        return ConvertResult<int>.OK(res);
                }),
                new ConvertRule<long>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<long>.NullInput();
                    else if (!long.TryParse(input, out long res))
                        return ConvertResult<long>.Incorrect();
                    else
                        return ConvertResult<long>.OK(res);
                }),
                new ConvertRule<float>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<float>.NullInput();
                    else if (!float.TryParse(input, out float res))
                        return ConvertResult<float>.Incorrect();
                    else
                        return ConvertResult<float>.OK(res);
                }),
                new ConvertRule<double>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<double>.NullInput();
                    else if (!double.TryParse(input, out double res))
                        return ConvertResult<double>.Incorrect();
                    else
                        return ConvertResult<double>.OK(res);
                }),
                new ConvertRule<bool>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<bool>.NullInput();
                    else if (!bool.TryParse(input, out bool res))
                        return ConvertResult<bool>.Incorrect();
                    else
                        return ConvertResult<bool>.OK(res);
                }),
                new ConvertRule<string>(
                (input) =>
                {
                    if (string.IsNullOrEmpty(input))
                        return ConvertResult<string>.NullInput();
                    else
                        return ConvertResult<string>.OK(input);
                })
            };
        }

        public void AddRule<TOut>(ConvertRule<TOut> rule, bool @override = false) where TOut : notnull
        {
            ConvertRule<TOut>? existing = FindRule<TOut>();
            if (existing is not null)
            {
                if (@override)
                {
                    Rules.Remove(existing);
                    Rules.Add(rule);
                }
                else throw new DuplicationException(GetType(), typeof(ConvertRule<TOut>), typeof(TOut).Name);
            }
            else Rules.Add(rule);
        }

        public void Clear() => Rules.Clear();

        public ConvertResult<TOut> Deserialize<TOut>(string input, char splitToken = ';') where TOut : notnull, new()
        {
            if (input is null) throw new ConvertNullInputException();
            List<string> args = input.Split(splitToken).ToList();

            //ConvertRule<TOut>? rule = ResolveTypeRule<TOut>();
            //if (rule is null)
            //    throw new Exception();

            TOut arsHolder = new();
            var propsLinks = typeof(TOut).GetProperties()
                .Where(x => x.GetCustomAttribute<BotActionArgumentAttribute>() is not null)
                .ToDictionary(x => x.GetCustomAttribute<BotActionArgumentAttribute>()!.ArgIndex);
            
            string _excepMes = string.Empty;
            if (args.Count != propsLinks.Count)
                _excepMes += Owner.ResolveDebugString("ai.display.ArgumentsCountMissmatch");

            for (int i = 0; i < propsLinks.Count; i++)
            {
                PropertyInfo prop = propsLinks[i];
                Type propType = prop.PropertyType;
                dynamic cvrtRule = Convert.ChangeType(
                    GetType().GetMethod(nameof(ResolveTypeRule))!
                    .MakeGenericMethod(propType).Invoke(this, null),
                    typeof(ConvertRule<>).MakeGenericType(propType))!;

                var cvrtRes = cvrtRule.Converter(args[i]);
                if (cvrtRes.ResultType == ConvertResultType.Ok)
                    prop.SetValue(arsHolder, cvrtRes.Value);
                else _excepMes += $"{cvrtRes.Message}\n";
            }

            return string.IsNullOrEmpty(_excepMes)
                ? ConvertResult<TOut>.OK(arsHolder)
                : ConvertResult<TOut>.Incorrect(_excepMes);
        }
        public string Serialize<TIn>(TIn input, char splitToken = ';') where TIn : notnull
        {
            string argLine = string.Empty;

            var propsLinks = typeof(TIn).GetProperties()
                .Where(x => x.GetCustomAttribute<BotActionArgumentAttribute>() is not null)
                .ToDictionary(x => x.GetCustomAttribute<BotActionArgumentAttribute>()!.ArgIndex);

            for (int i = 0; i < propsLinks.Count; i++)
            {
                PropertyInfo prop = propsLinks[i];
                Type propType = prop.PropertyType;
                argLine += $"{splitToken}{Pack(prop.GetValue(input))}";
            }

            return argLine;
        }

        public ConvertResult<TOut> Unpack<TOut>(string input) where TOut : notnull => ResolveTypeRule<TOut>().Converter(input);

        /// <summary>
        /// Converts an object of a type <typeparamref name="TIn"/> to a string that could be used as a representation of
        /// this object. Uses default ToString() method until <typeparamref name="TIn"/> is not <see cref="IArgPackable"/>.
        /// </summary>
        /// <typeparam name="TIn">A type of an object that should be packed.</typeparam>
        /// <param name="input"></param>
        /// <returns>A string that could be used as a representation of <paramref name="input"/> object.</returns>
        /// <exception cref="NullPackedException"></exception>
        public string Pack<TIn>(TIn input) => (input is IArgPackable pkg ? pkg.GetPacked() : input?.ToString())
            ?? throw new NullPackedException(typeof(TIn));
    }
}