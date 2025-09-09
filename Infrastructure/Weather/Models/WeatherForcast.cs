using System.Text.Json.Serialization;
using System.Xml;

namespace Infrastructure.Weather.Models
{
    public class WeatherForcast
    {
        [JsonPropertyName("coord")]
        public Coord? coord { get; set; }

        [JsonPropertyName("weather")]
        public Weather[] weather { get; set; } = Array.Empty<Weather>();
        [JsonPropertyName("base")]
        public string? _base { get; set; }
        [JsonPropertyName("main")]
        public Main? main { get; set; }
        [JsonPropertyName("visibility")]
        public int visibility { get; set; }
        [JsonPropertyName("wind")]
        public Wind? wind { get; set; }
        [JsonPropertyName("clouds")]
        public Clouds? clouds { get; set; }
        [JsonPropertyName("dt")]
        public long dt { get; set; }
        [JsonPropertyName("sys")]
        public Sys? sys { get; set; }
        [JsonPropertyName("timezone")]
        public int timezone { get; set; }
        public int id { get; set; }
        public string? name { get; set; }
        public int cod { get; set; }
    }

}
