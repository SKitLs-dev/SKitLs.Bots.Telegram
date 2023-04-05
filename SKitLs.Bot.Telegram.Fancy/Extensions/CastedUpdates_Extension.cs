//using SKitLs.Bots.Telegram.Core.Model.DelieverySystem.Model;
//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting;
//using SKitLs.Bots.Telegram.Core.Model.UpdatesCasting.Signed;
//using SKitLs.Bots.Telegram.Core.Prototypes;
//using SKitLs.Bots.Telegram.Fancy.Model;
//using Telegram.Bot;
//using Telegram.Bot.Types.InputFiles;

//namespace SKitLs.Bots.Telegram.Fancy.Extensions
//{
//    public static class CastedUpdates_Extension
//    {
//        public static Task<DelieveryResponse> SendMessageToSender(this SignedMessageUpdate update, OutputMessage message, CancellationTokenSource? cts = null)
//            => update.SendMessageToChatAsync(message, update.Sender.TelegramId, cts);

//        public static Task<DelieveryResponse> SendMessageToSender(this SignedCallbackUpdate update, OutputMessage message, CancellationTokenSource? cts = null)
//            => update.SendMessageToChatAsync(message, update.Sender.TelegramId, cts);

//        public static async Task<DelieveryResponse> SendMessageToTriggerChatAsync(this CastedChatUpdate update, OutputMessage message, CancellationTokenSource? cts = null)
//            => await update.SendMessageToChatAsync(message, update.ChatId, cts);

//        public static async Task<DelieveryResponse> SendMessageToChatAsync(this CastedUpdate update, OutputMessage message, long chatId, CancellationTokenSource? cts)
//        {
//            cts ??= new();
//            if (message is OMDTMedia omdt)
//            {
//                if (omdt.Type == MediaType.Photo && omdt.MediaTempFile != null)
//                {
//                    try
//                    {
//                        using (Stream reader = new FileStream(omdt.MediaTempFile, FileMode.Open))
//                        {
//                            await update.Bot.SendPhotoAsync(
//                                chatId: chatId,
//                                new InputOnlineFile(reader),
//                                caption: omdt.GetMessageText(),
//                                parseMode: omdt.ParseMode,
//                                replyToMessageId: omdt.ReplyToMessageId,
//                                replyMarkup: omdt.Markup,
//                                cancellationToken: cts.Token);
//                        }
//                        if (File.Exists(omdt.MediaTempFile))
//                            File.Delete(omdt.MediaTempFile);
//                        return DelieveryResponse.OK();
//                    }
//                    catch (Exception)
//                    {
//                        cts.Cancel();
//                        return DelieveryResponse.Forbidden();
//                    }
//                }
//                else if (omdt.Type == MediaType.Location)
//                {
//                    try
//                    {
//                        await update.Bot.SendLocationAsync(
//                            chatId: chatId,
//                            latitude: omdt.Latitude,
//                            longitude: omdt.Longtitude,
//                            replyToMessageId: omdt.ReplyToMessageId,
//                            replyMarkup: omdt.Markup,
//                            cancellationToken: cts.Token);
//                        return DelieveryResponse.OK();
//                    }
//                    catch (Exception)
//                    {
//                        cts.Cancel();
//                        return DelieveryResponse.Forbidden();
//                    }
//                }
//                else
//                {
//                    try
//                    {
//                        await update.Bot.SendTextMessageAsync(
//                            chatId: chatId,
//                            text: omdt.GetMessageText(),
//                            parseMode: omdt.ParseMode,
//                            replyToMessageId: omdt.ReplyToMessageId,
//                            replyMarkup: omdt.Markup,
//                            cancellationToken: cts.Token);
//                        return DelieveryResponse.OK();
//                    }
//                    catch (Exception)
//                    {
//                        cts.Cancel();
//                        return DelieveryResponse.Forbidden();
//                    }
//                }
//            }
//            else if (message is OutputMessageText text)
//            {
//                if (text is IOutputEdit editText)
//                {
//                    if (editText.EditMessageId >= 0)
//                    {
//                        try
//                        {
//                            await update.Bot.EditMessageTextAsync(
//                                chatId: chatId,
//                                messageId: editText.EditMessageId,
//                                text: text.GetMessageText(),
//                                parseMode: text.ParseMode,
//                                replyMarkup: editText.InlineMarkup,
//                                cancellationToken: cts.Token);
//                            return DelieveryResponse.OK();
//                        }
//                        catch (Exception)
//                        {
//                            cts.Cancel();
//                            return DelieveryResponse.Forbidden();
//                        }
//                    }
//                    else return DelieveryResponse.NoEditMessageId();
//                }
//                else
//                {
//                    try
//                    {
//                        await update.Bot.SendTextMessageAsync(
//                                chatId: chatId,
//                                text: text.GetMessageText(),
//                                parseMode: text.ParseMode,
//                                replyMarkup: text.Markup,
//                                replyToMessageId: text.ReplyToMessageId,
//                                cancellationToken: cts.Token);
//                        return DelieveryResponse.OK();
//                    }
//                    catch (Exception)
//                    {
//                        cts.Cancel();
//                        return DelieveryResponse.Forbidden();
//                    }
//                }
//            }
//            return DelieveryResponse.UnknownMessageType();
//        }
//    }
//}
