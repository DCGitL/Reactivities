namespace Infrastructure.Weather.Models.Response
{
    public class SysResponse
    {
        public string? Country { get; set; }
        public required string Sunrise { get; set; }
        public required string Sunset { get; set; }

    }

    public static class SysResponseExtensions
    {
        public static SysResponse ToSysResponse(this Sys sys)
        {
            var dateTimeSunRise = DateTimeOffset.FromUnixTimeSeconds(sys.sunrise).LocalDateTime;
            var dateTimeSunSet = DateTimeOffset.FromUnixTimeSeconds(sys.sunset).LocalDateTime;
            return new SysResponse
            {
                Country = sys.country,
                Sunrise = dateTimeSunRise.ToString("hh:mm:ss tt"),
                Sunset = dateTimeSunSet.ToString("hh:mm:ss tt")
            };
        }
    }

}
