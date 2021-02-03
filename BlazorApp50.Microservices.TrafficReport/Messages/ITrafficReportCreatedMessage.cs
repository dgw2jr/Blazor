using System;

namespace BlazorApp50.Microservices.TrafficReport.Messages
{
    public interface ITrafficReportCreatedMessage
    {
        string Summary { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
