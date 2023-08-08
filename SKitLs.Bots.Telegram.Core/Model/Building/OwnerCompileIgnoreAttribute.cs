namespace SKitLs.Bots.Telegram.Core.Model.Building
{
    // XML-Doc Update
    /// <summary>
    /// Determines that this property should be ignored during RecRef compilation of
    /// <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OwnerCompileIgnoreAttribute : Attribute { }
}