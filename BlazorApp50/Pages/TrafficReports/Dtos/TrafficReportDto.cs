using BlazorApp50.Microservices.Traffic.Data.Models;
using System;

namespace BlazorApp50.Pages.TrafficReports.Dtos
{
    public class TrafficReportDto
    {
        public TrafficReportDto()
        {

        }

        public TrafficReportDto(TrafficReport trafficReport) : this()
        {
            Id = trafficReport.Id;
            CreatedDate = trafficReport.CreatedDate;
            Summary = trafficReport.Summary;
        }

        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Summary { get; set; }
    }
}
