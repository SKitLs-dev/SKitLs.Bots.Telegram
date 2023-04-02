namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public class UpdateCastingException : SKTgException
    {
        public long UpdateId { get; set; }
        public string UpdateName { get; set; }

        public UpdateCastingException(string updateName, long updateId) : base(true, "UpdateCasting")
        {
            UpdateName = updateName;
            UpdateId = updateId;
        }
    }
}