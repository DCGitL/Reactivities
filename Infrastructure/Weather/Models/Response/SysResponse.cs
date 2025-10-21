using Infrastructure.Helper;

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
        public static SysResponse ToSysResponse(this Sys sys, float lonitude, float latitude)
        {

            return new SysResponse
            {
                Country = sys.country,
                Sunrise = TimeZoneConverter.GetLocalTimeString(sys.sunrise, latitude, lonitude),
                Sunset = TimeZoneConverter.GetLocalTimeString(sys.sunset, latitude, lonitude)
            };
        }
    }

}
