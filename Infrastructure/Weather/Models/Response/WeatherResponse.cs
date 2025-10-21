using System.Text.Json.Serialization;

namespace Infrastructure.Weather.Models.Response
{
    public class WeatherResponse
    {
        [JsonPropertyName("weather")]
        public Weather[] weather { get; set; } = Array.Empty<Weather>();
        public DateTime Dt { get; set; }
        public SysResponse? sys { get; set; }
        public MainResponse? main { get; set; }
        public string? City { get; set; }
        public string? LocalDateTime { get; set; }
        public string? Geolocation { get; set; }
        public string? StandardTimeZone { get; set; }

    }

}
