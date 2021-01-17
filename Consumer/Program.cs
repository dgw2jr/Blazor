using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Messages;

namespace Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    services.AddMassTransit(c =>
                    {
                        c.SetKebabCaseEndpointNameFormatter();

                        c.UsingRabbitMq((ctx, cfg) =>
                        {
                            cfg.Host("192.168.0.88", h =>
                            {
                                h.Username("user");
                                h.Password("BipyglxcSHK2");
                            });

                            cfg.ConfigureEndpoints(ctx);
                        });

                        c.AddConsumers(typeof(Program).Assembly);
                    });

                    services.AddMassTransitHostedService();
                }).RunConsoleAsync();
        }
    }

    public class WeatherReportCreatedConsumer : IConsumer<WeatherReportCreated>
    {
        public async Task Consume(ConsumeContext<WeatherReportCreated> context)
        {
            await Console.Out.WriteLineAsync($"Weather Report Received: {context.Message.Summary}");
        }
    }
}
