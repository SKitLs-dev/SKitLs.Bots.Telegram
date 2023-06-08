using System.Reflection;

namespace SKitLs.Bots.Telegram.Core.Model.Building
{
    /// <summary>
    /// Specified interface that used in reflective bot compilation <see cref="BotManager.ReflectiveCompile"/>.
    /// Used to specify certain instance <see cref="BotManager"/> owner.
    /// Autonomous module based on <see cref="System.Reflection"/>.
    /// </summary>
    public interface IOwnerCompilable
    {
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner { get; set; }

        /// <summary>
        /// Specified method that raised during reflective compilation.
        /// Declare it to extend preset functionality.
        /// </summary>
        public Action<object, BotManager>? OnCompilation { get; }

        /// <summary>
        /// Recursively and reflectively compiles all <see cref="IOwnerCompilable"/> properties
        /// that declared in <paramref name="sender"/> instance.
        /// </summary>
        /// <param name="sender">Instance that caused compilation. Used to get its properties' values.</param>
        /// <param name="owner">Global owner used to be declared</param>
        public void ReflectiveCompile(object sender, BotManager owner)
        {
            Owner = owner;
            OnCompilation?.Invoke(sender, owner);
            sender.GetType().GetProperties()
                .Where(x => x.GetCustomAttribute<OwnerCompilableIgnoreAttribute>() is null)
                .Where(x => x.PropertyType.GetInterfaces().Contains(typeof(IOwnerCompilable)))
                .ToList()
                .ForEach(refCompile =>
                {
                    var cmpVal = refCompile.GetValue(sender);
                    if (cmpVal is IOwnerCompilable oc)
                        oc.ReflectiveCompile(cmpVal, owner);
                });
        }
    }
}