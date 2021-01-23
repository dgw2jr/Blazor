using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class TrafficContext : DbContext
    {
        public TrafficContext()
        {

        }
        public TrafficContext(DbContextOptions<TrafficContext> options) : base(options)
        {

        }

        public DbSet<TrafficReport> TrafficReports { get; set; }
    }

    public class TrafficReport
    {
        public TrafficReport()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        public string Summary { get; set; }
    }
}
