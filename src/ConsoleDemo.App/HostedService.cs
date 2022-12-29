using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ConsoleDemo.Infrastructure;
using ConsoleDemo.Service;

namespace ConsoleDemo.App
{
    public class HostedService : IHostedService
    {
        private readonly ILogger<HostedService> _logger;
        private readonly IEltHostedServiceFactory _hostedServiceFactory;

        private string ServiceName { get; set; }

        public HostedService(
            ILogger<HostedService> logger,
            IEltHostedServiceFactory hostedServiceFactory)
        {
            _logger = logger;
            _hostedServiceFactory = hostedServiceFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                ServiceName = GetServiceNameFromEnvironmentVariables();
                _logger.LogInformation("{ServiceName}开始执行", ServiceName);
                var host = _hostedServiceFactory.GetService(ServiceName);
                host.StartAsync(cancellationToken);
                _logger.LogInformation("{ServiceName}执行结束", ServiceName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "服务异常:{Message}", e.Message);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("{ServiceName}停止执行", ServiceName);
                //Stop Something
            }
            catch (Exception e)
            {
                _logger.LogError(e, "服务异常:{Message}", e.Message);
            }

            return Task.CompletedTask;
        }

        private string GetServiceNameFromEnvironmentVariables()
        {
            var serviceName = EnvironmentVariablesExtension.ServiceName;
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                throw new ApplicationException("no serviceName found");
            }

            return serviceName;
        }
    }
}