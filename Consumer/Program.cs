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

                    services.AddMassTransit(c =>
                    {
                        c.SetKebabCaseEndpointNameFormatter();

                        //c.UsingRabbitMq((context, cfg) =>
                        //{
                        //    cfg.Host(ctx.Configuration.GetValue<string>("MassTransit:Host"), h =>
                        //    {
                        //        h.Username(ctx.Configuration.GetValue<string>("MassTransit:Username"));
                        //        h.Password(ctx.Configuration.GetValue<string>("MassTransit:Password"));
                        //    });

                        //    cfg.ConfigureEndpoints(context);
                        //});

                        c.UsingAzureServiceBus((context, cfg) =>
                        {
                            var settings = new HostSettings
                            {
                                ServiceUri = new Uri(ctx.Configuration.GetValue<string>("MassTransit:Host")),
                                TokenProvider = Microsoft.Azure.ServiceBus.Primitives.TokenProvider.CreateSharedAccessSignatureTokenProvider(ctx.Configuration.GetValue<string>("MassTransit:Username"),
                                ctx.Configuration.GetValue<string>("MassTransit:Password"))
                            };

                            cfg.Host(settings);
                            cfg.ConfigureEndpoints(context);
                        });

                        c.AddConsumers(typeof(Program).Assembly);
                    });

                    services.AddMassTransitHostedService();
                }).RunConsoleAsync();
        }
    }
}
