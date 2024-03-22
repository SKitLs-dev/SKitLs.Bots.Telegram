using System.Reflection;
using SKitLs.Bots.Telegram.Core.Model;

namespace SKitLs.Bots.Telegram.Core.Model.Building
{
    /// <summary>
    /// An interface used in the reflective bot compilation process (<see cref="BotManager.ReflectiveCompile"/>).
    /// Provides methods for specifying a certain instance of the <see cref="BotManager"/> class as the class's owner.
    /// Used for handlers, managers, and other services that should be able to access their owner instance after compilation.
    /// <para/>
    /// This interface represents a fully autonomous module based on <c><see cref="System.Reflection"/></c>.
    /// </summary>
    public interface IOwnerCompilable
    {
        /// <summary>
        /// The instance's owner.
        /// </summary>
        public BotManager Owner { get; set; }

        /// <summary>
        /// A method specified to be invoked during reflective compilation (<see cref="ReflectiveCompile(object, BotManager)"/>).
        /// Declare this method to extend preset functionality.
        /// Invoked after updating the <see cref="Owner"/> property, but before recursive update.
        /// </summary>
        public Action<object, BotManager>? OnCompilation { get; }

        /// <summary>
        /// Recursively and reflectively compiles all properties (declared in the <paramref name="sender"/> instance)
        /// that support the <see cref="IOwnerCompilable"/> interface, setting their <see cref="Owner"/> property to
        /// the <paramref name="owner"/> instance.
        /// <para/>
        /// Use the <see cref="OwnerCompileIgnoreAttribute"/> to prevent the update of the property's <see cref="Owner"/>
        /// and its reflective scanning.
        /// <para/>
        /// <b>Should not be overridden.</b>
        /// </summary>
        /// <param name="sender">The instance that caused the compilation. Used to get its properties' values.</param>
        /// <param name="owner">The global owner to be defined.</param>
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