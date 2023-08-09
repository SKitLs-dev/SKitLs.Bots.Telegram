using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Model;
using SKitLs.Bots.Telegram.ArgedInteractions.Argumentation.Prototype;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.External;
using SKitLs.Bots.Telegram.ArgedInteractions.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.ArgedInteractions.resources.settings;
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

        /// <inheritdoc/>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }
        
        /// <inheritdoc/>
        public Action<object, BotManager>? OnCompilation => null;
        
        /// <summary>
        /// Gets or sets the list of <see cref="ConvertRule"/> objects, that holds
        /// converting rules for different types.
        /// </summary>
        private List<ConvertRule> Rules { get; set; }

        /// <inheritdoc/>
        public ConvertRule<TOut> ResolveTypeRule<TOut>() where TOut : notnull
            => FindRule<TOut>() ?? throw new NotDefinedException(this, typeof(ConvertRule<TOut>), typeof(TOut).Name);
        /// <inheritdoc/>
        public ConvertRule<TOut>? FindRule<TOut>() where TOut : notnull
            => (ConvertRule<TOut>?)Rules.Find(r => r.ResultType == typeof(TOut));
        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
        
        /// <inheritdoc/>
        public void Clear() => Rules.Clear();

        /// <inheritdoc/>
        public ConvertResult<TOut> Deserialize<TOut>(string input, char splitToken = ';') where TOut : notnull, new()
            => DeserializeTo<TOut>(input, new(), splitToken);
        /// <inheritdoc/>
        public ConvertResult<TOut> DeserializeTo<TOut>(string input, TOut instance, char splitToken) where TOut : notnull
        {
            if (input is null) throw new ConvertNullInputException(this);
            List<string> args = input.Split(splitToken).ToList();

            //ConvertRule<TOut>? rule = ResolveTypeRule<TOut>();
            //if (rule is null)
            //    throw new Exception();

            var propsLinks = typeof(TOut).GetProperties()
                .Where(x => x.GetCustomAttribute<BotActionArgumentAttribute>() is not null)
                .ToDictionary(x => x.GetCustomAttribute<BotActionArgumentAttribute>()!.ArgIndex);
            
            string _exceptionMes = string.Empty;
            if (args.Count != propsLinks.Count)
                _exceptionMes += Owner.ResolveDebugString(SKaiSettings.ArgumentsCountMissMatchLK);
            else
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
                        prop.SetValue(instance, convertRes.Value);
                    else _exceptionMes += $"{convertRes.Message}\n";
                }

            return string.IsNullOrEmpty(_exceptionMes)
                ? ConvertResult<TOut>.OK(instance)
                : ConvertResult<TOut>.Incorrect(_exceptionMes);
        }
        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public ConvertResult<TOut> Unpack<TOut>(string input) where TOut : notnull => ResolveTypeRule<TOut>().Converter(input);
        /// <inheritdoc/>
        /// <remarks>
        /// Supports <see cref="IArgPackable"/>.
        /// </remarks>
        public string Pack<TIn>(TIn input) => (input is IArgPackable pkg ? pkg.GetPacked() : input?.ToString())
            ?? throw new NullPackedException(this, typeof(TIn));
    }
}