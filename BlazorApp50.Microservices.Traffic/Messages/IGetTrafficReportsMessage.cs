using BlazorApp50.Microservices.Traffic.Data.Models;
using System.Collections.Generic;

namespace BlazorApp50.Microservices.Traffic.Messages
{
    public interface IGetTrafficReportsMessage
    {
        int? Count { get; set; }
    }

    public interface IGetTrafficReportsResult
    {
        List<TrafficReport> TrafficReports { get; set; }
    }
}
