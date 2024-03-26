using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    /// <summary>
    /// Represents a wrapper class that adds localization functionality to a buildable content object.
    /// </summary>
    /// <typeparam name="TContent">The type of the buildable content object.</typeparam>
    /// <typeparam name="TBuildTemp">Real type of the build result.</typeparam>
    /// <typeparam name="TBuildResult">Assigned type of the build result.</typeparam>
    public class Localized<TContent, TBuildTemp, TBuildResult> : IBuildableContent<TBuildResult> where TContent : notnull, IBuildableContent<TBuildResult>, ICloneable
    {
        /// <summary>
        /// Gets or sets the value of the buildable content object.
        /// </summary>
        public TContent Value { get; set; }

        /// <summary>
        /// Gets or sets the name of the property to localize within the buildable content object.
        /// </summary>
        public string LocalizePropertyName { get; set; }

        /// <summary>
        /// Gets or sets the list of format arguments used for formatting the localized text.
        /// </summary>
        public string?[] FormatArgsList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Localized{TContent, TBuildTemp, TBuildResult}"/> class with the specified parameters.
        /// </summary>
        /// <param name="value">The buildable content object.</param>
        /// <param name="localizePropertyName">The name of the property to localize within the buildable content object.</param>
        /// <param name="format">The list of format arguments used for formatting the localized text.</param>
        public Localized(TContent value, string localizePropertyName, params string?[] format)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            LocalizePropertyName = localizePropertyName;
            FormatArgsList = format;
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This method resolves a localized string by replacing the value of the specified property within the buildable content object.
        /// </remarks>
        public async Task<TBuildResult> BuildContentAsync(ICastedUpdate? update)
        {
            var result = await Value.BuildContentAsync(update);
            var localizedProperty = typeof(TBuildTemp).GetProperty(LocalizePropertyName);
            var propValue = localizedProperty?.GetValue(result)?.ToString();
            if (update is not null && localizedProperty is not null && propValue is not null)
            {
                var localized = update.Owner.ResolveBotString(propValue, FormatArgsList) ?? propValue;
                localizedProperty.SetValue(result, localized);
            }
            return result;
        }

        /// <inheritdoc/>
        public object Clone() => new Localized<TContent, TBuildTemp, TBuildResult>((TContent)Value.Clone(), (string)LocalizePropertyName.Clone(), FormatArgsList);
    }
}