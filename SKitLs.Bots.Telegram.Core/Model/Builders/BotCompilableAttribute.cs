namespace SKitLs.Bots.Telegram.Core.Model.Builders
{
    [Obsolete($"Replaced with {nameof(IOwnerCompilable)}", true)]
    [AttributeUsage(AttributeTargets.Property)]
    internal class BotCompilableAttribute : Attribute
    {
        public BotCompilableAttribute() { }
    }
}