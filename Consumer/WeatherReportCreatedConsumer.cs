using System;
using System.Threading.Tasks;
using MassTransit;
using Messages;

namespace Consumer
{
    public class WeatherReportCreatedConsumer : IConsumer<WeatherReportCreated>
    {
        public async Task Consume(ConsumeContext<WeatherReportCreated> context)
        {
            await Console.Out.WriteLineAsync($"Weather Report Received: {context.Message.Summary}");
        }
    }
}
