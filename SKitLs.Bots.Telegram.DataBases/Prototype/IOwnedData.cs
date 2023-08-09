namespace SKitLs.Bots.Telegram.DataBases.Prototype
{
    public interface IOwnedData
    {
        public bool IsOwnedBy(long userId);
    }
}