using System;
using System.Linq;

namespace ConsoleDemo.Service
{
    public class EltHostedServiceFactory : IEltHostedServiceFactory
    {
        private readonly IEtlHostedService[] _hostedServices;

        public EltHostedServiceFactory(IEtlHostedService[] hostedServices)
        {
            if (hostedServices == null || !hostedServices.Any())
            {
                throw new NotImplementedException($"IEtlHostedService was not implemented");
            }

            _hostedServices = hostedServices;
        }

        public IEtlHostedService GetService(string serviceName)
        {
            var service = _hostedServices.FirstOrDefault(x => x.Name == serviceName);
            if (service == null)
            {
                throw new NotImplementedException($"No services with the serviceName '{serviceName}' were found");
            }

            return service;
        }
    }
}