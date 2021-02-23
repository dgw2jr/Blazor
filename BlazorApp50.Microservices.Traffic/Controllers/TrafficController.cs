using BlazorApp50.Microservices.Traffic.Data;
using BlazorApp50.Microservices.Traffic.Data.Models;
using BlazorApp50.Microservices.Traffic.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp50.Microservices.Traffic.Controllers
{
    [ApiController]
    [Route("api/trafficreports")]
    public class TrafficController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly TrafficContext _trafficContext;

        public TrafficController(IBus bus, TrafficContext trafficContext)
        {
            _bus = bus;
            _trafficContext = trafficContext;
        }

        /// <summary>
        /// Get traffic reports
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrafficReport>))]
        public async Task<IActionResult> Get()
        {
            var reports = await _trafficContext.TrafficReports
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return Ok(reports);
        }

        /// <summary>
        /// Publishes an <see cref="ITrafficReportCreatedMessage"/> to the service bus for processing
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> Create([FromBody]TrafficReport dto)
        {
            await _bus.Publish<ITrafficReportCreatedMessage>(new
            {
                Summary = dto.Summary,
                CreatedDate = dto.CreatedDate
            });

            return Accepted();
        }
    }
}
