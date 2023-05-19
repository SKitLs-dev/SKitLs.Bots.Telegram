namespace SKitLs.Bots.Telegram.Core.Prototypes
{
    public interface IApplyable<TFor>
    {
        public void ApplyFor(TFor source);
    }
}