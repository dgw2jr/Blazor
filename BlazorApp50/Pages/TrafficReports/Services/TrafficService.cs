using BlazorApp50.Extensions;
using BlazorApp50.Microservices.Traffic.Data.Dtos;
using Newtonsoft.Json;
using System.Collections.Generic;
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

        public async Task<List<TrafficReportDto>> GetTrafficReports()
        {
            var response = await _httpClient.GetAsync("/api/trafficreports");
            return await response.ReadContentAs<List<TrafficReportDto>>();
        }
    }

    public interface ITrafficService
    {
        Task<List<TrafficReportDto>> GetTrafficReports();
    }
}
