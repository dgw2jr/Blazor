using System;
using System.Threading.Tasks;
using MassTransit;
using Messages;
using Core;

namespace Consumer
{
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
}
