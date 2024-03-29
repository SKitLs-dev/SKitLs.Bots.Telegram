﻿using SKitLs.Bots.Telegram.Core.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model;
using SKitLs.Bots.Telegram.Core.UpdatesCasting;

namespace SKitLs.Bots.Telegram.Core.DeliverySystem.Model
{
    /// <summary>
    /// Represents a message builder that derives from <see cref="TelegramTextMessage"/> and implements
    /// the <see cref="IBuildableMessage"/> interface.
    /// This class is designed to dynamically build message content based on an update.
    /// </summary>
    public class BuildableMessage : TelegramTextMessage, IBuildableMessage
    {
        private readonly UpdateBasedTask<BuildableMessage, ICastedUpdate, string> _messageBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildableMessage"/> class with the specified message builder.
        /// </summary>
        /// <param name="messageBuilder">The function that dynamically builds the message content based on an update.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="messageBuilder"/> is null.</exception>
        public BuildableMessage(UpdateBasedTask<BuildableMessage, ICastedUpdate, string> messageBuilder)
        {
            _messageBuilder = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
        }

        /// <inheritdoc/>
        public override object Clone() => new BuildableMessage(_messageBuilder)
        {
            AllowSendingWithoutReply = AllowSendingWithoutReply,
            DisableNotification = DisableNotification,
            DisableWebPagePreview = DisableWebPagePreview,
            ParseMode = ParseMode,
            ProtectContent = ProtectContent,
            ReplyMarkup = ReplyMarkup,
            ReplyToMessageId = ReplyToMessageId,
        };

        /// <inheritdoc/>
        public virtual async Task<ITelegramMessage> BuildContentAsync(ICastedUpdate? update)
        {
            Text = await _messageBuilder.Invoke(this, update);
            return this;
        }
    }
}