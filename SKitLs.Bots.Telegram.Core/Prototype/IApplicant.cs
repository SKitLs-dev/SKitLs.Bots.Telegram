using SKitLs.Bots.Telegram.Core.Model.Interactions;
using SKitLs.Bots.Telegram.Core.Model.Management;

namespace SKitLs.Bots.Telegram.Core.Prototype
{
    /// <summary>
    /// Generic interface that provides methods of integrating external code to library interior.
    /// Can be implemented in your custom class, which could be applied to a custom <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Service, manager or other class that implementor should be applied to.</typeparam>
    public interface IApplicant<T>
    {
        /// <summary>
        /// Applies inherited class to <paramref name="entity"/>, defining and integrating necessary data to it.
        /// <para>
        /// For example, if <see cref="IApplicant{T}"/> is applicable to <see cref="IActionManager{TUpdate}"/>
        /// use <see cref="IActionManager{TUpdate}.AddSafely(IBotAction{TUpdate})"/> to integrate all <see cref="IBotAction{TUpdate}"/>,
        /// defined in applicant.
        /// </para>
        /// </summary>
        /// <param name="entity">An instance that this class should be applied to.</param>
        public void ApplyTo(T entity);
    }
}