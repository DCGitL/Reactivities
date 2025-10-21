using Infrastructure.TimeZone.Models;

namespace Infrastructure.TimeZone;

public interface IGeoTimeZoneService
{
    Task<TimeZoneResponse?> GetTimeZoneResponseAsync(float lat, float lon);
}
