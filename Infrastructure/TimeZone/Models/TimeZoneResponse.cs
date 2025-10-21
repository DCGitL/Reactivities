using System.Text.Json.Serialization;

namespace Infrastructure.TimeZone.Models
{
    public class TimeZoneResponse
    {
        [JsonPropertyName("time_zone")]
        public Time_Zone? TimeZone { get; set; }
    }

}
