using Infrastructure.TimeZone.Models;

namespace Infrastructure.TimeZone;

public interface IGeoTimeZoneService
{
    Task<TimeZoneResponse?> GetTimeZoneResponseAsync(double lat, double lon);
}
