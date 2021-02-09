using BlazorApp50.Microservices.Traffic.Data;
using BlazorApp50.Microservices.Traffic.Data.Models;
using BlazorApp50.Microservices.Traffic.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp50.Microservices.Traffic.Consumers
{
    public class GetTrafficReportsConsumer : IConsumer<IGetTrafficReportsMessage>
    {
        private readonly TrafficContext _trafficContext;

        public GetTrafficReportsConsumer(TrafficContext trafficContext)
        {
            _trafficContext = trafficContext;
        }

        public async Task Consume(ConsumeContext<IGetTrafficReportsMessage> context)
        {
            await Console.Out.WriteLineAsync($"{context.MessageId}: Getting Traffic Reports");

            var reports = _trafficContext.TrafficReports
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedDate);

            if (context.Message.Count != null)
            {
                reports = (IOrderedQueryable<TrafficReport>)reports.Take(context.Message.Count.Value);
            }

            var result = await reports.ToListAsync();

            await Console.Out.WriteLineAsync($"{context.Message.GetType().Name}: Returning {result.Count} results");

            await context.RespondAsync<IGetTrafficReportsResult>(new { TrafficReports = result });
        }
    }
}
