using BlazorApp50.Microservices.Traffic.Data.Dtos;
using BlazorApp50.Microservices.Traffic.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp50.Microservices.Traffic.Controllers
{
    [ApiController]
    [Route("api/trafficreports")]
    public class TrafficController : ControllerBase
    {
        private readonly IRequestClient<IGetTrafficReportsMessage> _requestClient;

        public TrafficController(IRequestClient<IGetTrafficReportsMessage> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<TrafficReportDto>>> Get()
        {
            var response = await _requestClient.GetResponse<IGetTrafficReportsResult>(new { });
            var reports = response.Message.TrafficReports;

            return reports
                .Select(r => new TrafficReportDto(r))
                .ToList();
        }
    }
}
