using System;

namespace BlazorApp50.Microservices.Traffic.Messages
{
    public interface ITrafficReportCreatedMessage
    {
        string Summary { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
