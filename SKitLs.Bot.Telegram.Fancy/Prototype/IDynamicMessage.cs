using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Prototype
{
    /// <summary>
    /// Provides mechanics of creating dynamic messages, whose content can be updated and overridden during runtime.
    /// </summary>
    public interface IDynamicMessage
    {
        ///// <summary>
        ///// Represents specific method that can generate message's content, based on incoming update.
        ///// </summary>
        //public Func<ISignedUpdate?, IOutputMessage> MessageBuilder { get; }

        /// <summary>
        /// Generates new message content, based on an incoming <paramref name="update"/>.
        /// </summary>
        /// <param name="update">An incoming update.</param>
        /// <returns>Updated with <paramref name="update"/> message.</returns>
        public IOutputMessage BuildWith(ISignedUpdate? update);
    }
}