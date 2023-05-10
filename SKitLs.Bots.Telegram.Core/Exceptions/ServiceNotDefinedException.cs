namespace SKitLs.Bots.Telegram.Core.Exceptions
{
    public class ServiceNotDefinedException : SKTgException
    {
        public Type ServiceType { get; set; }

        public ServiceNotDefinedException(Type service) : base(true, "ServiceNotDefined", service.Name)
        {
            ServiceType = service;
        }
    }
}
