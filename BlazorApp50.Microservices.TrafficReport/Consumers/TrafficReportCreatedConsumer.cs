using BlazorApp50.Microservices.TrafficReport.Messages;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace BlazorApp50.Microservices.TrafficReport.Consumers
{
    public class TrafficReportCreatedConsumer : IConsumer<ITrafficReportCreatedMessage>
    {
        public async Task Consume(ConsumeContext<ITrafficReportCreatedMessage> context)
        {
            await Console.Out.WriteLineAsync($"Traffic Report Received: {context.Message.Summary}");
        }
    }
}
