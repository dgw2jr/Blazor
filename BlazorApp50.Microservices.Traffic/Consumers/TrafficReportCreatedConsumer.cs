using BlazorApp50.Microservices.Traffic.Data;
using BlazorApp50.Microservices.Traffic.Data.Models;
using BlazorApp50.Microservices.Traffic.Messages;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace BlazorApp50.Microservices.Traffic.Consumers
{
    public class TrafficReportCreatedConsumer : IConsumer<ITrafficReportCreatedMessage>
    {
        private readonly TrafficContext _trafficContext;

        public TrafficReportCreatedConsumer(TrafficContext trafficContext)
        {
            _trafficContext = trafficContext;
        }

        public async Task Consume(ConsumeContext<ITrafficReportCreatedMessage> context)
        {
            await Console.Out.WriteLineAsync($"{context.MessageId}: Saving to database");

            await _trafficContext.AddAsync(new TrafficReport
            {
                CreatedDate = context.Message.CreatedDate,
                Summary = context.Message.Summary,
            });

            await _trafficContext.SaveChangesAsync();

            await Console.Out.WriteLineAsync($"{context.MessageId} Saved successfully!");
        }
    }
}
