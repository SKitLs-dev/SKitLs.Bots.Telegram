namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception that occurs when trying to resolve a service that does not exist.
    /// </summary>
    public class ServiceNotDefinedException : SKTgSignedException
    {
        /// <summary>
        /// The type of service that was attempted to be resolved.
        /// </summary>
        public Type ServiceType { get; private init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceNotDefinedException"/> class with specified data.
        /// </summary>
        /// <param name="sender">The object that has thrown the exception.</param>
        /// <param name="serviceType">The type of service that was attempted to be resolved.</param>
        public ServiceNotDefinedException(object sender, Type serviceType)
            : base("ServiceNotDefined", SKTEOriginType.External, sender, serviceType.Name)
        {
            ServiceType = serviceType;
        }
    }
}