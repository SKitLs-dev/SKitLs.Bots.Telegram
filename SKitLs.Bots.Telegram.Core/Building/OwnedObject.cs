using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Model;

namespace SKitLs.Bots.Telegram.Core.Building
{
    /// <summary>
    /// Represents an abstract class that serves as a base for objects owned by <see cref="BotManager"/>.
    /// </summary>
    public abstract class OwnedObject : IOwnerCompilable
    {
        private BotManager? _owner;

        /// <inheritdoc/>
        public BotManager Owner
        {
            get => _owner ?? throw new NullOwnerException(this);
            set => _owner = value;
        }

        /// <inheritdoc/>
        public virtual Action<object, BotManager>? OnCompilation => null;
    }
}
