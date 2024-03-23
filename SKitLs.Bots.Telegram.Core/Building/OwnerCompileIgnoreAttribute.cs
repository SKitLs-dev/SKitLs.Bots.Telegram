using SKitLs.Bots.Telegram.Core.Model;

namespace SKitLs.Bots.Telegram.Core.Building
{
    /// <summary>
    /// Specifies that this property should be ignored during the reflective compilation
    /// process of <see cref="IOwnerCompilable.ReflectiveCompile(object, BotManager)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OwnerCompileIgnoreAttribute : Attribute { }
}