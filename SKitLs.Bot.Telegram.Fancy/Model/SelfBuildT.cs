using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model
{
    /// <summary>
    /// Represents a self-buildable content provider. Uses itself <typeparamref name="T"/> when building content.
    /// </summary>
    /// <typeparam name="T">The type of the value to build.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SelfBuild{T}"/> class with the specified value.
    /// </remarks>
    /// <param name="value">The value to build.</param>
    public class SelfBuild<T>(T value) : IBuildableContent<T>
    {
        /// <summary>
        /// Gets or sets the value to build.
        /// </summary>
        public T Value { get; set; } = value;

        /// <inheritdoc/>
        public async Task<T> BuildContentAsync(ICastedUpdate? update) => await Task.FromResult(Value);

        /// <inheritdoc/>
        public object Clone() => new SelfBuild<T>(Value is ICloneable cloneable ? (T)cloneable.Clone() : Value);
    }

    /// <summary>
    /// Represents a self-buildable message. Custom extension for <see cref="SelfBuild{T}"/> with <see cref="IBuildableMessage"/> specified.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SelfBuildMessage"/> class with the specified value.
    /// </remarks>
    /// <param name="value">The message value to build.</param>
    public class SelfBuildMessage(ITelegramMessage value) : SelfBuild<ITelegramMessage>(value), IBuildableMessage
    { }
}