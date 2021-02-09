using System;
using System.Threading.Tasks;
using MassTransit;
using Messages;
using Core;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Consumer
{
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
