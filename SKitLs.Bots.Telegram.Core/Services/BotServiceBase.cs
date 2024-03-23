using SKitLs.Bots.Telegram.Core.Building;

namespace SKitLs.Bots.Telegram.Core.Services
{
    /// <summary>
    /// Represents a base class for bot services.
    /// </summary>
    public abstract class BotServiceBase : OwnedObject, IBotService
    {
        private string? _debugName;

        /// <inheritdoc/>
        public virtual string? DebugName
        {
            get => _debugName ?? GetType().Name;
            protected set => _debugName = value;
        }
    }
}
