using System.Net.Http.Json;
using API.Helper;
using Infrastructure.Helper;
using Infrastructure.TimeZone;
using Infrastructure.Weather.Models;
using Infrastructure.Weather.Models.Response;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Weather.WeatherService
{

    public class WeatherServiceMonitor(IHttpClientFactory httpClientFactory, IConfiguration configuration, IGeoTimeZoneService geoTimeZoneService) : IWeatherServiceMonitor
    {
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IConfiguration configuration = configuration;

        public async Task<WeatherResponse?> GetWeatherForcast(double lat, double lon, CancellationToken cancellationToken)
        {
            var endpoint = string.Format(configuration.GetSection("WeatherApi:ApiEndPoint").Value!, lat, lon);
            var client = httpClientFactory.CreateClient(HttpClientName.WeatherSerivceClient.ToString());
            var response = await client.GetAsync(endpoint, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var weatherForcast = await response.Content.ReadFromJsonAsync<WeatherForcast>();
                if (weatherForcast == null) return null;
                var geolocation = await GetGeolocationLocalTime(lat, lon);
                var weatherResponse = new WeatherResponse
                {
                    weather = weatherForcast.weather,
                    LocalDateTime = geolocation.localTime,
                    Geolocation = geolocation.geolocation,
                    StandardTimeZone = geolocation.standardTimezone,

                    Dt = TimeZoneConverter.GetLocalDateTime(weatherForcast.dt, lat, lon),
                    main = weatherForcast.main?.ToMainResponse(),
                    sys = weatherForcast.sys?.ToSysResponse(lon, lat),
                    City = weatherForcast.name
                };
                foreach (var weather in weatherForcast.weather)
                {
                    weather.icon = string.Format(configuration.GetSection("WeatherApi:IconSrc").Value!, weather.icon);
                }
                ;
                if (weatherResponse.sys != null && !string.IsNullOrEmpty(weatherResponse?.sys?.Country))
                {
                    weatherResponse.sys.Country = GetCountyName(weatherResponse.sys.Country);
                }
                return weatherResponse;
            }
            return null;

        }


        private string GetCountyName(string countryCode)
        {
            return new System.Globalization.RegionInfo(countryCode).EnglishName;
        }

        private async Task<(string localTime, string geolocation, string standardTimezone)> GetGeolocationLocalTime(double lat, double lon)
        {
            var result = await geoTimeZoneService.GetTimeZoneResponseAsync(lat, lon);
            if (result is not null)
            {
                return (localTime: $"{result?.TimeZone?.date} {result?.TimeZone?.time_12}",
                 geolocation: $"{result?.TimeZone?.name}",
                 standardTimezone: $"{result?.TimeZone?.standard_tz_full_name}");
            }

            return ("", "", "");
        }
    }


}
