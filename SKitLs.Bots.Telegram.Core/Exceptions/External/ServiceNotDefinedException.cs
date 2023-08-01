namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception which occurs when trying to resolve an service that does not exist.
    /// </summary>
    public class ServiceNotDefinedException : SKTgSignedException
    {
        /// <summary>
        /// Type of a service that was tried to be resolved.
        /// </summary>
        public Type ServiceType { get; private init; }

        /// <summary>
        /// Creates a new instance of <see cref="ServiceNotDefinedException"/> with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown exception.</param>
        /// <param name="serviceType">Type of a service that was tried to be resolved.</param>
        public ServiceNotDefinedException(object sender, Type serviceType)
            : base("ServiceNotDefined", SKTEOriginType.External, sender, serviceType.Name)
        {
            ServiceType = serviceType;
        }
    }
}