namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    /// <summary>
    /// An exception which occurs when trying to resolve an service that does not exist.
    /// </summary>
    public class ServiceNotDefinedException : SKTgException
    {
        /// <summary>
        /// Type of a service that was tried to be resolved.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="ServiceNotDefinedException"/> with specified data.
        /// </summary>
        /// <param name="serviceType">Type of a service that was tried to be resolved.</param>
        public ServiceNotDefinedException(Type serviceType) : base("ServiceNotDefined", SKTEOriginType.External, serviceType.Name)
        {
            ServiceType = serviceType;
        }
    }
}