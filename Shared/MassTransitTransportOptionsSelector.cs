using MassTransit;
using MassTransit.Azure.ServiceBus.Core.Configurators;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Shared
{
    public enum MassTransitTransportOptions
    {
        RabbitMq,
        AzureServiceBus
    }

    public static class MassTransitTransportOptionsSelector
    {
        private static Dictionary<MassTransitTransportOptions, Action<IServiceCollectionBusConfigurator>> _strategies = new Dictionary<MassTransitTransportOptions, Action<IServiceCollectionBusConfigurator>>
        {
            [MassTransitTransportOptions.RabbitMq] = (x) => x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(ctx.GetService<IConfiguration>().GetValue<string>("MassTransit:Host"), h =>
                {
                    h.Username(ctx.GetService<IConfiguration>().GetValue<string>("MassTransit:Username"));
                    h.Password(ctx.GetService<IConfiguration>().GetValue<string>("MassTransit:Password"));
                });

                cfg.ConfigureEndpoints(ctx);
            }),
            [MassTransitTransportOptions.AzureServiceBus] = (x) => x.UsingAzureServiceBus((ctx, cfg) =>
            {
                var settings = new HostSettings
                {
                    ServiceUri = new Uri(ctx.GetService<IConfiguration>().GetValue<string>("MassTransit:Host")),
                    TokenProvider = Microsoft.Azure.ServiceBus.Primitives.TokenProvider.CreateSharedAccessSignatureTokenProvider(ctx.GetService<IConfiguration>().GetValue<string>("MassTransit:Username"), ctx.GetService<IConfiguration>().GetValue<string>("MassTransit:Password"))
                };

                cfg.Host(settings);
                cfg.ConfigureEndpoints(ctx);
            })
        };

        public static void UseTransport(this IServiceCollectionBusConfigurator serviceCollectionBusConfigurator, MassTransitTransportOptions transportOptions)
        {
            _strategies[transportOptions](serviceCollectionBusConfigurator);
        }

        public static IServiceCollection UseMassTransit(this IServiceCollection services, Action<IServiceCollectionBusConfigurator> serviceCollectionBusConfigurator)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.UseTransport(MassTransitTransportOptions.AzureServiceBus);

                serviceCollectionBusConfigurator(x);
            });

            return services;
        }
    }
}
