using MassTransit;
using MassTransit.Azure.ServiceBus.Core.Configurators;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Shared
{
    public static class MassTransitTransportOptionsSelector
    {
        public static Action<IServiceCollectionBusConfigurator> RabbitMq = x => x.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(ctx.GetService<IConfiguration>().GetValue<string>("MassTransitRabbitMQ:Host"), h =>
            {
                h.Username(ctx.GetService<IConfiguration>().GetValue<string>("MassTransitRabbitMQ:Username"));
                h.Password(ctx.GetService<IConfiguration>().GetValue<string>("MassTransitRabbitMQ:Password"));
            });

            cfg.ConfigureEndpoints(ctx);
        });

        public static Action<IServiceCollectionBusConfigurator> AzureServiceBus = x => x.UsingAzureServiceBus((ctx, cfg) =>
        {
            var settings = new HostSettings
            {
                ServiceUri = new Uri(ctx.GetService<IConfiguration>().GetValue<string>("MassTransitAzureSB:Host")),
                TokenProvider = Microsoft.Azure.ServiceBus.Primitives.TokenProvider.CreateSharedAccessSignatureTokenProvider(ctx.GetService<IConfiguration>().GetValue<string>("MassTransitAzureSB:Username"), ctx.GetService<IConfiguration>().GetValue<string>("MassTransitAzureSB:Password"))
            };

            cfg.Host(settings);
            cfg.ConfigureEndpoints(ctx);
        });

        private static void UseTransport(this IServiceCollectionBusConfigurator serviceCollectionBusConfigurator, Action<IServiceCollectionBusConfigurator> configurator)
        {
            configurator(serviceCollectionBusConfigurator);
        }

        public static IServiceCollection UseMassTransit(this IServiceCollection services, Action<IServiceCollectionBusConfigurator> serviceCollectionBusConfigurator)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UseTransport(RabbitMq);

                serviceCollectionBusConfigurator(x);
            });

            return services;
        }
    }
}
