using SKitLs.Bots.Telegram.Core.Exceptions;
using SKitLs.Bots.Telegram.Core.Model.Building;
using SKitLs.Utils.LocalLoggers.Prototype;
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

        private static string Local(ILocalizedLogger logger, string mesKey, params string?[] format)
            => logger.Localizator.ResolveString(BotBuilder.DebugSettings.DebugLanguage, mesKey, format);
        public static void Log(this ILocalizedLogger logger, Exception exception)
        {
            string errorMes = "Exception was thrown: ";
            string? warn = null;
            if (exception is SKTgException sktg)
            {
                errorMes += "SKitLs.Bots.Telegram Error\n";
                errorMes += $"\n{Local(logger, sktg.CaptionLocalKey)}\n";
                errorMes += $"{Local(logger, sktg.MessgeLocalKey, sktg.Format)}";

                warn = sktg.OriginType switch
                {
                    SKTEOriginType.Internal => Local(logger, "system.InternalExcep"),
                    SKTEOriginType.Inexternal => Local(logger, "system.InexternalExcep"),
                    SKTEOriginType.External => Local(logger, "system.ExternalExcep"),
                    _ => null,
                };
            }
            else if (exception is ApiRequestException apiRequestException)
            {
                errorMes += "Telegram API Error\n";
                errorMes += $"\n[{apiRequestException.ErrorCode}]\n" +
                    $"{apiRequestException.Message}";
            }
            else
            {
                errorMes += "Native C# Error\n";
                errorMes += $"\n{exception.Message} ({exception.Source})\n";
            }

            if (BotBuilder.DebugSettings.ShouldPrintExceptionTrace)
                errorMes += $"\n{exception.StackTrace}";
            
            logger.Error(errorMes);
            if (warn is not null)
                logger.Warn(warn);
        }
        public static void Log(this ILocalizedLogger logger, Update update, bool warn = false)
        {
            string sender = string.Empty;
            string content = string.Empty;
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
                content = string.Empty;
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
                content = string.Empty;
            }
            else
            {
                sender = string.Empty;
                content = string.Empty;
            }
            string text = Local(logger, "system.UpdateMessage", DateTime.Now.ToString("G"),
                update.Id.ToString(), update.Type.ToString(), sender, content);
            if (warn) logger.Warn(text);
            else logger.Log(text);
        }
    }
}