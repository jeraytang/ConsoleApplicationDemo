using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ConsoleDemo.App.Extensions;
using ConsoleDemo.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConsoleDemo.Infrastructure;
using ConsoleDemo.Service;
using Serilog;
using Serilog.Events;

namespace ConsoleDemo.App
{
    class Program
    {
        private static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            var logFile = Environment.GetEnvironmentVariable("LOG");
            if (string.IsNullOrEmpty(logFile))
            {
                logFile = Path.Combine(AppContext.BaseDirectory, "OneData.BiEtl.App.log");
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console().WriteTo.RollingFile(logFile)
                .CreateLogger();

            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    Configuration = config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile("appsettings." + hostContext.HostingEnvironment + ".json", true, true)
                        .AddEnvironmentVariables()
                        .Build();
                })
                .ConfigureContainer<ContainerBuilder>((hostContext, containerBuilder) =>
                {
                    containerBuilder.RegisterModule<ConfigureAutofac>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<HostedService>()
                        .AddAutoMapper(typeof(AutoMapperProfile))
                        .AddOptions()
                        .AddMemoryCache()
                        .AddHttpClient()
                        .AddScoped<IEltHostedServiceFactory, EltHostedServiceFactory>()
                        .AddScoped<AppOptions>();
                    

                    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
                    Dapper.SqlMapper.AddTypeHandler(new DateTimeOffsetHandler());
                    Dapper.SqlMapper.AddTypeHandler(new StringGuidHandler());
                })
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
