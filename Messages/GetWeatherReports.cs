using Core;
using System.Collections.Generic;

namespace Messages
{
    public interface GetWeatherReports
    {
        int? Count { get; set; }
    }

    public interface GetWeatherReportsResult
    {
        List<WeatherReport> WeatherReports { get; set; }
    }
}
