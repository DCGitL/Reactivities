using System;
using System.Globalization;
using GeoTimeZone;
using NodaTime;

namespace Infrastructure.Helper;

public class TimeZoneConverter
{
    public static string GetLocalTimeString(long unixTimestamp, double latitude, double longitude)
    {
        try
        {
            // Convert Unix timestamp to NodaTime Instant
            var instant = Instant.FromUnixTimeSeconds(unixTimestamp);

            // Get timezone ID from coordinates using GeoTimeZone - CORRECT METHOD
            TimeZoneResult timeZoneId = TimeZoneLookup.GetTimeZone(latitude, longitude);

            // Get the timezone and convert to local time
            var zone = DateTimeZoneProviders.Tzdb[timeZoneId.Result];
            var localTime = instant.InZone(zone);

            // Return formatted time
            return localTime.ToString("h:mm tt", CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Timezone lookup failed: {ex.Message}");

            // Fallback to UTC
            var utcTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
            return utcTime.ToString("h:mm tt") + " (UTC Fallback)";
        }
    }

    public static DateTime GetLocalDateTime(long unixTimestamp, double latitude, double longitude)
    {
        try
        {
            // Convert Unix timestamp to NodaTime Instant
            var instant = Instant.FromUnixTimeSeconds(unixTimestamp);

            // Get timezone ID from coordinates using GeoTimeZone - CORRECT METHOD
            TimeZoneResult timeZoneId = TimeZoneLookup.GetTimeZone(latitude, longitude);

            // Get the timezone and convert to local time
            var zone = DateTimeZoneProviders.Tzdb[timeZoneId.Result];
            var localTime = instant.InZone(zone);

            // Return DateTime
            return localTime.ToDateTimeUnspecified();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Timezone lookup failed: {ex.Message}");

            // Fallback to UTC
            return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).UtcDateTime;
        }
    }

}
