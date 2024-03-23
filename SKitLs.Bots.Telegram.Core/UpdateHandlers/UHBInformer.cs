using SKitLs.Bots.Telegram.Core.Building;
using SKitLs.Bots.Telegram.Core.Exceptions.Inexternal;
using SKitLs.Bots.Telegram.Core.Exceptions.Internal;
using SKitLs.Bots.Telegram.Core.Interactions;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;
using SKitLs.Bots.Telegram.Core.Users;

namespace SKitLs.Bots.Telegram.Core.UpdateHandlers
{
    /// <summary>
    /// A default informer class that informs about incoming updates.
    /// <para>
    /// This class is only intended for debugging purposes.
    /// </para>
    /// </summary>
    /// <typeparam name="TUpdate">The specific casted update type that this handler should work with.</typeparam>
    /// <remarks>
    /// Creates a new instance of the <see cref="UHBInformer{TUpdate}"/> class with the specified data.
    /// </remarks>
    /// <param name="updateName">The name of the update to be printed.</param>
    /// <param name="log">Specifies whether information should be printed in the logger.</param>
    /// <param name="inform">Specifies whether information should be printed in the chat.</param>
    [Obsolete("Unsafe. Only use for debugging purposes.")]
    public class UHBInformer<TUpdate>(string? updateName = null, bool log = true, bool inform = false)
        : OwnedObject, IUpdateHandlerBase<TUpdate> where TUpdate : class, ICastedUpdate
    {
        /// <summary>
        /// Gets or sets the name of the update to be printed.
        /// </summary>
        public string UpdateName { get; init; } = updateName ?? typeof(TUpdate).Name;

        /// <summary>
        /// Determines whether information about the update should be printed in the logger.
        /// </summary>
        public bool UseLogger { get; init; } = log;

        /// <summary>
        /// Determines whether information about the update should be printed in the chat.
        /// </summary>
        public bool InformInChat { get; init; } = inform;

        /// <inheritdoc/>
        public List<IBotAction> GetHeldActions() => [];

        /// <inheritdoc/>
        public async Task HandleUpdateAsync(ICastedUpdate update, IBotUser? sender) => await HandleUpdateAsync(CastUpdate(update, sender));

        /// <inheritdoc/>
        public TUpdate CastUpdate(ICastedUpdate update, IBotUser? sender) => update as TUpdate
            ?? throw new BotManagerException("UHBCasting", UpdateName, update.OriginalSource.Id.ToString());

        /// <inheritdoc/>
        public async Task HandleUpdateAsync(TUpdate update)
        {
            string mes = $"Handled update (by {nameof(UHBInformer<TUpdate>)}): {UpdateName}";
            if (UseLogger) { Owner.LocalLogger.Log(mes); }
            if (InformInChat) await Owner.DeliveryService.SendMessageToChatAsync(update.ChatId, mes);
        }
    }
}