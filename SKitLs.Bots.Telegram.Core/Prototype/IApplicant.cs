using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;

namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// A generic interface that provides methods for integrating custom functionality into an external class or service,
    /// typically applied to a custom <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the service, manager, or other class to which the implementor should be applied.</typeparam>
    public interface IApplicant<T>
    {
        /// <summary>
        /// Applies the functionality defined in the implementing class to an instance of <paramref name="entity"/>,
        /// integrating any necessary data or behavior.
        /// <para/>
        /// For example, if <see cref="IApplicant{T}"/> is applicable to <see cref="IActionManager{TUpdate}"/>,
        /// use <see cref="IActionManager{TUpdate}.AddSafely(IBotAction{TUpdate})"/> to integrate all <see cref="IBotAction{TUpdate}"/>
        /// defined in the implementing class.
        /// </summary>
        /// <param name="entity">An instance of the class or service to which this functionality should be applied.</param>
        public void ApplyTo(T entity);
    }
}