using System;

namespace Messages
{
    public interface WeatherReportCreated
    {
        decimal TemperatureF { get; set; }
        decimal DewPointF { get; set; }
        string Summary { get; set; }
        int WindSpeedMPH { get; set; }
        string WindDirection { get; set; }

        DateTime CreatedDate { get; set; }
    }
}
