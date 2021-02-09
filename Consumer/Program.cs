using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.IO;
using MassTransit.Azure.ServiceBus.Core.Configurators;
using System;
using Shared;

namespace Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                        .AddJsonFile("CommonSettings.json", optional: true)
                        .AddJsonFile($"CommonSettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    config.Build();
                })
                .ConfigureServices((ctx, services) =>
                {
                    services.AddDbContext<WeatherContext>(options => options.UseSqlServer(ctx.Configuration.GetConnectionString("WeatherContext")));

                    services.UseMassTransit(c =>
                    {
                        c.AddConsumers(typeof(Program).Assembly);
                    });

                    services.AddMassTransitHostedService();
                }).RunConsoleAsync();
        }
    }
}
