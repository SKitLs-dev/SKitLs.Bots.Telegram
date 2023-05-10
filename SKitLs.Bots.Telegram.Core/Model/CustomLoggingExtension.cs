using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.external.LocalizedLoggers;
using SKitLs.Bots.Telegram.Core.external.Loggers;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SKitLs.Bots.Telegram.Core.Model
{
    public static class CustomLoggingExtension
    {
        private static string NoTitle => "NoTitle";
        private static string NoUserTag => "NoTag";
        private static int MaxMessageContentLength => 16;

        public static void Log(this ILocalizedLogger logger, Exception exception)
        {
            string errorMes = exception switch
            {
                SKTgException sktg => $"SKitLs.Bots.Telegram Error:" +
                $"\n> {logger.Localizator.ResolveString(logger.Owner.Settings.DebugLanguage, sktg.LocalKey, sktg.Format)}" +
                $"{(logger.Owner.DebugSettings.ShouldPrintExceptionTrace ? $"\n{sktg.StackTrace}" : string.Empty)}",
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n" +
                    $"{apiRequestException.Message}",
                _ => $"{exception.Message} ({exception.Source})" +
                    $"\n{exception.StackTrace}",
            };
            logger.Error(errorMes);
        }
        public static void Log(this ILocalizedLogger logger, Update update, bool warn = false)
        {
            string sender = " - From: ";
            string content = " - Content: ";
            if (update.Type == UpdateType.Message)
            {
                sender += $"@{update.Message!.From?.Username ?? NoUserTag} ({update.Message!.Chat.Id})";
                content += update.Message!.Type == MessageType.Text
                    ? update.Message.Text!.Length > MaxMessageContentLength
                        ? $"{update.Message.Text[..MaxMessageContentLength]}"
                        : $"{update.Message.Text}"
                    : $"{update.Message.Type}";
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                sender += $"@{update.CallbackQuery!.Message!.Chat.Username ?? NoUserTag} ({update.CallbackQuery!.Message!.Chat.Id})";
                content += $"{update.CallbackQuery.Data}";
            }
            else if (update.Type == UpdateType.ChannelPost)
            {
                sender += $"{update.ChannelPost!.Chat.Title ?? NoTitle} ({update.ChannelPost.Chat.Id})";
                content += update.ChannelPost.Type == MessageType.Text
                    ? update.ChannelPost.Text!.Length > MaxMessageContentLength
                        ? $"{update.ChannelPost.Text[..MaxMessageContentLength]}"
                        : $"{update.ChannelPost.Text}"
                    : $"{update.ChannelPost.Type}";
            }
            else if (update.Type == UpdateType.ChatMember)
            {
                sender += $"{update.ChatMember!.Chat.Title ?? NoTitle} ({update.ChatMember.Chat.Id})";
                content = "";
            }
            else if (update.Type == UpdateType.ChosenInlineResult)
            {
                sender += $"@{update.ChosenInlineResult!.From.Username ?? NoUserTag} ({update.ChosenInlineResult!.From.Id})";
                content += $"{update.ChosenInlineResult!.ResultId}";
            }
            else if (update.Type == UpdateType.EditedChannelPost)
            {
                sender += $"{update.EditedChannelPost!.Chat.Title ?? NoTitle} ({update.EditedChannelPost!.Chat.Id})";
                content += update.EditedChannelPost.Type == MessageType.Text
                    ? update.EditedChannelPost.Text!.Length > MaxMessageContentLength
                        ? $"{update.EditedChannelPost.Text[..MaxMessageContentLength]}"
                        : $"{update.EditedChannelPost.Text}"
                    : $"{update.EditedChannelPost.Type}";
            }
            else if (update.Type == UpdateType.EditedMessage)
            {
                sender += $"@{update.EditedMessage!.Chat.Username ?? NoUserTag} ({update.EditedMessage!.Chat.Id})";
                content += update.EditedMessage.Type == MessageType.Text
                    ? update.EditedMessage.Text!.Length > MaxMessageContentLength
                        ? $"{update.EditedMessage.Text[..MaxMessageContentLength]}"
                        : $"{update.EditedMessage.Text}"
                    : $"{update.EditedMessage.Type}";
            }
            else if (update.Type == UpdateType.InlineQuery)
            {
                sender += $"@{update.InlineQuery!.From.Username ?? NoUserTag} ({update.InlineQuery!.From.Id})";
                content += $"{update.InlineQuery!.Query}";
            }
            else if (update.Type == UpdateType.MyChatMember)
            {
                sender += $"{update.MyChatMember!.Chat.Title ?? NoTitle} ({update.MyChatMember.Chat.Id})";
                content = "";
            }
            else
            {
                sender = "";
                content = "";
            }
            string text = $"[{DateTime.Now:G}] ({update.Id}) Received: {update.Type}{sender}{content}";

            if (warn) logger.Warn(text);
            else logger.Log(text);
        }
    }
}
