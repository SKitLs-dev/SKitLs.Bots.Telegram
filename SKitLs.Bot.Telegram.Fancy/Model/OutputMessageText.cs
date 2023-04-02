namespace SKitLs.Bots.Telegram.Fancy.Model
{
    public abstract class OutputMessageText : OutputMessage
    {
        public int ReplyToMessageId { get; set; }

        public abstract string GetMessageText();
    }
}
