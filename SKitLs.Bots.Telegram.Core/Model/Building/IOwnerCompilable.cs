using System.Reflection;

namespace SKitLs.Bots.Telegram.Core.Model.Building
{
    /// <summary>
    /// An interface that used in the reflective bot compilation <see cref="BotManager.ReflectiveCompile"/>.
    /// Provides methods of specifing a certain instance of a <see cref="BotManager"/> class as the class's owner.
    /// Used for handlers, managers and other services that should be able to access their owner instance after its compilation.
    /// <para>
    /// Fully autonomous module based on <c><see cref="System.Reflection"/></c>.
    /// </para>
    /// </summary>
    public interface IOwnerCompilable
    {
        /// <summary>
        /// Instance's owner.
        /// </summary>
        public BotManager Owner { get; set; }

        /// <summary>
        /// Specified method that raised during reflective <see cref="ReflectiveCompile(object, BotManager)"/> compilation.
        /// Declare it to extend preset functionality.
        /// Invoked after <see cref="Owner"/> updating, but before recursive update.
        /// </summary>
        public Action<object, BotManager>? OnCompilation { get; }

        /// <summary>
        /// Recursively and reflectively compiles all properties (declared in a <paramref name="sender"/> instance)
        /// that supports <see cref="IOwnerCompilable"/> interface, setting their <see cref="Owner"/> property as
        /// <paramref name="owner"/>.
        /// <para>
        /// Use <see cref="OwnerCompileIgnoreAttribute"/> to prevent property's <see cref="Owner"/> update
        /// and its reflective scanning.
        /// </para>
        /// <para>
        /// Should <c>not</c> be overriden.
        /// </para>
        /// </summary>
        /// <param name="sender">Instance that caused compilation. Used to get its properties' values.</param>
        /// <param name="owner">Global owner used to be defined</param>
        public void ReflectiveCompile(object sender, BotManager owner)
        {
            Owner = owner;
            OnCompilation?.Invoke(sender, owner);
            sender.GetType().GetProperties()
                .Where(x => x.GetCustomAttribute<OwnerCompileIgnoreAttribute>() is null)
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