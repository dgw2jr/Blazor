using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(cfg => cfg.Build())
                .ConfigureServices((ctx, services) =>
                {
                    services.AddDbContext<WeatherContext>(options => options.UseSqlServer(ctx.Configuration.GetConnectionString("WeatherContext")));

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

    public class WeatherReportCreatedConsumer2 : IConsumer<WeatherReportCreated>
    {
        private readonly WeatherContext _weatherContext;

        public WeatherReportCreatedConsumer2(WeatherContext weatherContext)
        {
            _weatherContext = weatherContext;
        }

        public async Task Consume(ConsumeContext<WeatherReportCreated> context)
        {
            await Console.Out.WriteLineAsync($"{context.MessageId}: Saving to database");

            await _weatherContext.AddAsync(new WeatherReport
            {
                CreatedDate = context.Message.CreatedDate,
                DewPointF = context.Message.DewPointF,
                Summary = context.Message.Summary,
                TemperatureF = context.Message.TemperatureF
            });

            await _weatherContext.SaveChangesAsync();

            await Console.Out.WriteLineAsync($"{context.MessageId} Saved successfully!");
        }
    }

    public class GetWeatherReportsConsumer : IConsumer<GetWeatherReports>
    {
        private readonly WeatherContext _weatherContext;

        public GetWeatherReportsConsumer(WeatherContext weatherContext)
        {
            _weatherContext = weatherContext;
        }

        public async Task Consume(ConsumeContext<GetWeatherReports> context)
        {
            var reports = _weatherContext.WeatherReports.OrderByDescending(r => r.CreatedDate);

            if(context.Message.Count != null)
            {
                reports = (IOrderedQueryable<WeatherReport>)reports.Take(context.Message.Count.Value);
            }

            var result = await reports.ToListAsync();

            await Console.Out.WriteLineAsync($"{context.Message.GetType().Name}: Returning {result.Count} results");

            await context.RespondAsync<GetWeatherReportsResult>(new { WeatherReports = result });
        }
    }
}
