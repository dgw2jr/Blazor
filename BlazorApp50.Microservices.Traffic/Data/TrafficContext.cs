using Microsoft.EntityFrameworkCore;

namespace BlazorApp50.Microservices.Traffic.Data
{
    public class TrafficContext : DbContext
    {
        public TrafficContext()
        {

        }
        public TrafficContext(DbContextOptions<TrafficContext> options) : base(options)
        {

        }

        public DbSet<Models.TrafficReport> TrafficReports { get; set; }
    }
}
