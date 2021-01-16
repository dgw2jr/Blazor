using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp50.Data
{
    public class WeatherContext : DbContext
    {
        public WeatherContext()
        {

        }
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {

        }

        DbSet<WeatherReport> WeatherReports { get; set; }
    }

    public class WeatherReport
    {
        public WeatherReport()
        {
            WeatherReportId = Guid.NewGuid();
        }

        public Guid WeatherReportId { get; private set; }
        public decimal TemperatureF { get; set; }
        public decimal DewPointF { get; set; }
        public string Summary { get; set; }
        public int WindSpeedMPH { get; set; }
        public string WindDirection { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
