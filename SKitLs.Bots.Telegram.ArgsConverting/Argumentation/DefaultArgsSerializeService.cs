using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.External;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.External;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.Model.Building;
using System.Reflection;

namespace SKitLs.Bots.Telegram.ArgedInteractions.Argumentation
{
    /// <summary>
    /// Represents default realization of <see cref="IArgsSerializeService"/> and provides methods
    /// to serialize and deserialize method arguments, using Converting System Rules.
    /// <para>
    /// <seealso cref="ConvertRule{TOut}"/>, <seealso cref="ConvertResult{TOut}"/>
    /// </para>
    /// </summary>
    public class DefaultArgsSerializeService : IArgsSerializeService
    {
        private BotManager? _owner;
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        /// <summary>
        /// Specified method that raised during reflective <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/> compilation.
        /// Declare it to extend preset functionality.
        /// Invoked after <see cref="Owner"/> updating, but before recursive update.
        /// </summary>
        public Action<object, BotManager>? OnCompilation => null;
        
        /// <summary>
        /// Gets or sets the list of <see cref="ConvertRule"/> objects, that holds
        /// converting rules for different types.
        /// </summary>
        private List<ConvertRule> Rules { get; set; }

        /// <summary>
        /// Gets the conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>. If doesn't exist - exception is raised.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>The conversion rule for <typeparamref name="TOut"/> type.</returns>
        public ConvertRule<TOut> ResolveTypeRule<TOut>() where TOut : notnull
            => FindRule<TOut>() ?? throw new NotDefinedException(this, typeof(ConvertRule<TOut>), typeof(TOut).Name);

        /// <summary>
        /// Tries to get the conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>The conversion rule for <typeparamref name="TOut"/> type.</returns>
        public ConvertRule<TOut>? FindRule<TOut>() where TOut : notnull
            => (ConvertRule<TOut>?)Rules.Find(r => r.ResultType == typeof(TOut));

        /// <summary>
        /// Determines whether a conversion rule <see cref="ConvertResult{TOut}"/> is defined for the specified
        /// output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns><see langword="true"/> if requested rule is determined. Otherwise <see langword="false"/>.</returns>
        public bool IsDefined<TOut>() where TOut : notnull => FindRule<TOut>() is not null;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultArgsSerializeService"/> class
        /// with preset conversion rules for <see cref="int"/>, <see cref="long"/>,
        /// <see cref="float"/>, <see cref="double"/>, <see cref="bool"/> and <see cref="string"/>
        /// </summary>
        public DefaultArgsSerializeService()
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

        /// <summary>
        /// Adds new conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>.
        /// <para>
        /// Attempting to add a rule for existing without an override flag will result in an error.
        /// </para>
        /// </summary>
        /// <typeparam name="TOut">Type of the converter.</typeparam>
        /// <param name="rule">Rule to be added.</param>
        /// <param name="override">Determines whether rule should be overridden, if one exists.</param>
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

        /// <summary>
        /// Clears all defined conversion rules.
        /// </summary>
        public void Clear() => Rules.Clear();

        /// <summary>
        /// Deserializes the specified input string to the output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Output type.</typeparam>
        /// <param name="input">Input argument string.</param>
        /// <param name="splitToken">Represents a token that the data is separated with.</param>
        /// <returns>Conversion result.</returns>
        public ConvertResult<TOut> Deserialize<TOut>(string input, char splitToken = ';') where TOut : notnull, new()
        {
            if (input is null) throw new ConvertNullInputException(this);
            List<string> args = input.Split(splitToken).ToList();

            //ConvertRule<TOut>? rule = ResolveTypeRule<TOut>();
            //if (rule is null)
            //    throw new Exception();

            TOut arsHolder = new();
            var propsLinks = typeof(TOut).GetProperties()
                .Where(x => x.GetCustomAttribute<BotActionArgumentAttribute>() is not null)
                .ToDictionary(x => x.GetCustomAttribute<BotActionArgumentAttribute>()!.ArgIndex);
            
            string _exceptionMes = string.Empty;
            if (args.Count != propsLinks.Count)
                _exceptionMes += Owner.ResolveDebugString("ai.display.ArgumentsCountMissMatch");

            for (int i = 0; i < propsLinks.Count; i++)
            {
                PropertyInfo prop = propsLinks[i];
                Type propType = prop.PropertyType;
                dynamic convertRule = Convert.ChangeType(
                    GetType().GetMethod(nameof(ResolveTypeRule))!
                    .MakeGenericMethod(propType).Invoke(this, null),
                    typeof(ConvertRule<>).MakeGenericType(propType))!;

                var convertRes = convertRule.Converter(args[i]);
                if (convertRes.ResultType == ConvertResultType.Ok)
                    prop.SetValue(arsHolder, convertRes.Value);
                else _exceptionMes += $"{convertRes.Message}\n";
            }

            return string.IsNullOrEmpty(_exceptionMes)
                ? ConvertResult<TOut>.OK(arsHolder)
                : ConvertResult<TOut>.Incorrect(_exceptionMes);
        }
        /// <summary>
        /// Serializes the specified input object.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="input">Input argument string.</param>
        /// <param name="splitToken">Represents a token that the data is separated with.</param>
        /// <returns>A string that represents serialized <paramref name="input"/>.</returns>
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

        /// <summary>
        /// Creates a new object of a type <typeparamref name="TOut"/>, converting it from <paramref name="input"/> data
        /// with help of appropriate <see cref="ConvertRule{TOut}"/>.
        /// </summary>
        /// <typeparam name="TOut">A type of an object that should be unpacked.</typeparam>
        /// <param name="input">String data that should be converted to an instance of a type <typeparamref name="TOut"/>.</param>
        /// <returns><see cref="ConvertResult{TOut}"/> that contains information about converting attempt.</returns>
        public ConvertResult<TOut> Unpack<TOut>(string input) where TOut : notnull => ResolveTypeRule<TOut>().Converter(input);

        /// <summary>
        /// Converts an object of a type <typeparamref name="TIn"/> to a string that could be used as a representation of
        /// this object. Uses default ToString() method until <typeparamref name="TIn"/> is not <see cref="IArgPackable"/>.
        /// </summary>
        /// <typeparam name="TIn">A type of an object that should be packed.</typeparam>
        /// <param name="input">.</param>
        /// <returns>A string that could be used as a representation of <paramref name="input"/> object.</returns>
        /// <exception cref="NullPackedException"></exception>
        public string Pack<TIn>(TIn input) => (input is IArgPackable pkg ? pkg.GetPacked() : input?.ToString())
            ?? throw new NullPackedException(this, typeof(TIn));
    }
}