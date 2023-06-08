namespace SKitLs.Bots.Telegram.Core.Exceptions.Internal
{
    public class UpdateCastingException : SKTgException
    {
        public long UpdateId { get; set; }
        public string UpdateName { get; set; }

        public UpdateCastingException(string updateName, long updateId) : base("UpdateCasting", SKTEOriginType.Internal)
        {
            UpdateName = updateName;
            UpdateId = updateId;
        }
    }
}