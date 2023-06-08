namespace SKitLs.Bots.Telegram.Core.Exceptions.External
{
    public class ServiceNotDefinedException : SKTgException
    {
        public Type ServiceType { get; set; }

        public ServiceNotDefinedException(Type service) : base("ServiceNotDefined", SKTEOriginType.External, service.Name)
        {
            ServiceType = service;
        }
    }
}