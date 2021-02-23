using BlazorApp50.Extensions;
using BlazorApp50.Microservices.Traffic.Data.Models;
using BlazorApp50.Pages.TrafficReports.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp50.Pages.TrafficReports.Services
{
    public class TrafficService : ITrafficService
    {
        private readonly HttpClient _httpClient;

        public TrafficService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TrafficReportDto>> GetTrafficReportDtos()
        {
            var response = await _httpClient.GetAsync("/api/trafficreports/");
            var trafficReports = await response.ReadContentAs<List<TrafficReport>>();

            return trafficReports.Select(tr => new TrafficReportDto(tr)).ToList();
        }

        public async Task<HttpResponseMessage> CreateTrafficReport(TrafficReportDto report)
        {
            //TODO Error checking
            report.CreatedDate = DateTime.UtcNow;

            var response = await _httpClient.PostAsJson($"api/trafficreports/create", report);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Could not create traffic report.");


            return response;
        }
    }

    public interface ITrafficService
    {
        Task<HttpResponseMessage> CreateTrafficReport(TrafficReportDto report);
        Task<List<TrafficReportDto>> GetTrafficReportDtos();
    }
}
