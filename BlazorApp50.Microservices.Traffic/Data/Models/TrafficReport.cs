using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp50.Microservices.Traffic.Data.Models
{
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
