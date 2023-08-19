using SKitLs.Bots.Telegram.AdvancedMessages.Prototype;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Model;
using SKitLs.Bots.Telegram.Core.Model.DeliverySystem.Prototype;
using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
using System.Text.RegularExpressions;
using Telegram.Bot.Types.Enums;
using TEnum = Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text
{
    /// <summary>
    /// Specific <see cref="OutputMessage{TMessage}"/> that provides formatted text, consisting of multiple text blocks.
    /// Uses only <see cref="ParseMode.Markdown"/>.
    /// </summary>
    public class MultiblockMessage : OutputMessage<MultiblockMessage>
    {
        /// <inheritdoc/>
        /// <remarks>
        /// Works only in <see cref="ParseMode.Markdown"/>. Attempt of modifying will result in <see cref="NotImplementedException"/>.
        /// </remarks>
        /// <exception cref="NotImplementedException">Thrown when </exception>
        public override ParseMode? ParseMode
        {
            get => TEnum.ParseMode.Markdown;
            set => throw new NotImplementedException();
        }

        /// <summary>
        /// Special message's leading block, marked as Bold.
        /// </summary>
        public string? Header { get; set; }

        /// <summary>
        /// Message's text blocks, separated by double '\n' in a result message.
        /// </summary>
        public List<string> Sections { get; set; } = new();

        /// <summary>
        /// Special message's closing block, marked as Italic.
        /// </summary>
        public string? Footer { get; set; }

        /// <summary>
        /// Basic constructor for the <see cref="MultiblockMessage"/> class.
        /// </summary>
        public MultiblockMessage() { }
        /// <summary>
        /// Constructor for the <see cref="MultiblockMessage"/> class with a specified body.
        /// </summary>
        /// <param name="body">The body of the message, containing sections separated by double '\n'.</param>
        public MultiblockMessage(string body) => Sections = Regex.Split(body, "\n{2,}").ToList();
        /// <summary>
        /// Constructor for the <see cref="MultiblockMessage"/> class with a specified menu.
        /// </summary>
        /// <param name="menu">The menu associated with the message.</param>
        public MultiblockMessage(IBuildableContent<IMessageMenu> menu) => Menu = menu ?? throw new ArgumentNullException(nameof(menu));

        /// <summary>
        /// Adds a new section block to the message's content.
        /// </summary>
        /// <param name="block">The content of the section block to be added.</param>
        public void AddBlock(string block) => Sections.Add(block);

        /// <summary>
        /// Collects header, footer and all the defined section blocks into final string body;
        /// </summary>
        /// <returns>A message body.</returns>
        public string GetBody()
        {
            string text = string.Empty;
            if (Header is not null) text += $"*{Header}*\n\n";
            foreach (string section in Sections)
                if (section is not null) text += $"{section}\n\n";
            if (Footer is not null) text += $"_{Footer}_";
            return text;
        }

        /// <inheritdoc/>
        public override async Task<ITelegramMessage> BuildContentAsync(ICastedUpdate? update)
        {
            var message = ContentBuilder is not null ? await ContentBuilder.Invoke(this, update) : this;
            var menu = Menu is not null ? await Menu.BuildContentAsync(update) : null;
            var text = message.GetBody();

            return new TelegramTextMessage(text)
            {
                AllowSendingWithoutReply = AllowSendingWithoutReply,
                DisableNotification = DisableNotification,
                DisableWebPagePreview = DisableWebPagePreview,
                ParseMode = ParseMode,
                ProtectContent = ProtectContent,
                ReplyMarkup = menu?.GetMarkup(),
                ReplyToMessageId = ReplyToMessageId,
            };
        }

        /// <inheritdoc/>
        public override object Clone()
        {
            var _sec = new List<string>();
            Sections.ForEach(x => _sec.Add((string)x.Clone()));
            return new MultiblockMessage()
            {
                ReplyToMessageId = ReplyToMessageId,
                Header = (string?)Header?.Clone(),
                Sections = _sec,
                Footer = (string?)Footer?.Clone(),
                ParseMode = ParseMode,
                Menu = (IBuildableContent<IMessageMenu>?)Menu?.Clone()
            };
        }
    }
}