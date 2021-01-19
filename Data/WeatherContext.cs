using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core
{
    public class WeatherContext : DbContext
    {
        public WeatherContext()
        {

        }
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {

        }

        public DbSet<WeatherReport> WeatherReports { get; set; }
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

        [Required]
        public string Summary { get; set; }
        public int WindSpeedMPH { get; set; }
        public string WindDirection { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
