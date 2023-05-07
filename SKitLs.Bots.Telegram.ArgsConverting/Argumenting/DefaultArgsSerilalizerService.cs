using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumenting.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions;
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
        /// <summary>
        /// Gets or sets the list of <see cref="ConvertRule"/> objects, that holds
        /// converting rules for different types.
        /// </summary>
        private List<ConvertRule> Rules { get; set; }

        /// <summary>
        /// Gets the conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>The conversion rule for <typeparamref name="TOut"/> type.</returns>
        public ConvertRule<TOut> ResolveTypeRule<TOut>()
        {
            ConvertRule? rule = Rules.Find(r => r.OutType == typeof(TOut));

            if (rule is null)
                throw new Exception();

            if (rule is not ConvertRule<TOut>)
                throw new Exception();

            return (ConvertRule<TOut>)rule; //Convert.ChangeType(rule, typeof(ConvertRule<TOut>), null);
        }

        /// <summary>
        /// Determines whether a conversion rule <see cref="ConvertResult{TOut}"/> is defined for the specified
        /// output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of the requested converter.</typeparam>
        /// <returns>True if requested rule is determined. Otherwise False.</returns>
        public bool IsDefined<TOut>() => ResolveTypeRule<TOut>() is not null;

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

        /// <summary>
        /// Adds new conversion rule <see cref="ConvertResult{TOut}"/> for the specified
        /// output type <typeparamref name="TOut"/>.
        /// <para>
        /// Attempting to add a rule for existing without an override flag will result in an error.
        /// </para>
        /// </summary>
        /// <typeparam name="TOut">Type of the converter.</typeparam>
        /// <param name="rule">Rule to be added.</param>
        /// <param name="override">If already exists, should be overriden.</param>
        public void AddRule<TOut>(ConvertRule<TOut> rule, bool @override = false)
        {
            ConvertRule<TOut>? existing = ResolveTypeRule<TOut>();
            if (existing is not null)
            {
                if (@override)
                {
                    Rules.Remove(existing);
                    Rules.Add(rule);
                }
                else throw new Exception();
            }
            else Rules.Add(rule);
        }

        /// <summary>
        /// Clears all defined conversion rules.
        /// </summary>
        public void Clear() => Rules.Clear();

        /// <summary>
        /// Serializes the specified input object.
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Serialize<TIn>(TIn input, char splitToken = ';')
        {
            string argLine = string.Empty;

            var propsLinks = typeof(TIn).GetProperties()
                .Where(x => x.GetCustomAttribute<BotActionArgumentAttribute>() is not null)
                .ToDictionary(x => x.GetCustomAttribute<BotActionArgumentAttribute>()!.ArgIndex);

            for (uint i = 0; i < propsLinks.Count; i++)
            {
                PropertyInfo prop = propsLinks[i];
                Type propType = prop.PropertyType;
                argLine += $"{splitToken}{prop.GetValue(input)}";
            }

            return argLine;
        }

        /// <summary>
        /// Deserializes the specified input string to the output type <typeparamref name="TOut"/>.
        /// </summary>
        /// <typeparam name="TOut">Output type.</typeparam>
        /// <param name="input">Input argument string.</param>
        /// <returns>Conversion result.</returns>
        public ConvertResult<TOut> Deserialize<TOut>(string input, char splitToken = ';') where TOut : new()
        {
            if (input is null)
                throw new ConvertNullInputException();
            List<string> args = input.Split(splitToken).ToList();

            ConvertRule<TOut>? rule = ResolveTypeRule<TOut>();
            if (rule is null)
                throw new Exception();

            TOut arsHolder = new();
            var propsLinks = typeof(TOut).GetProperties()
                .Where(x => x.GetCustomAttribute<BotActionArgumentAttribute>() is not null)
                .ToDictionary(x => x.GetCustomAttribute<BotActionArgumentAttribute>()!.ArgIndex);

            for (uint i = 0; i < propsLinks.Count; i++)
            {
                PropertyInfo prop = propsLinks[i];
                Type propType = prop.PropertyType;
                var cvrtRule = (ConvertRule<TOut>)Convert.ChangeType(
                    GetType().GetMethod(nameof(ResolveTypeRule))!
                    .MakeGenericMethod(propType).Invoke(this, null),
                    typeof(ConvertRule<>).MakeGenericType(propType))!;

                prop.SetValue(arsHolder, cvrtRule.Converter(args[(int)i]));
            }

            return ConvertResult<TOut>.OK(arsHolder);
        }

        public ConvertResult<TOut> Unpack<TOut>(string input)
        {
            var rule = ResolveTypeRule<TOut>();
            return rule.Converter(input);
        }

        public string Pack<TIn>(TIn input)
        {
            return input?.ToString() ?? throw new Exception();
        }
    }
}