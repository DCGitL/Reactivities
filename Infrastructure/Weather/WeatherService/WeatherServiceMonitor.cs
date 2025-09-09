using System.Net.Http.Json;
using Infrastructure.Weather.Models;
using Infrastructure.Weather.Models.Response;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Weather.WeatherService
{

    public class WeatherServiceMonitor(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IWeatherServiceMonitor
    {
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly IConfiguration configuration = configuration;

        public async Task<WeatherResponse?> GetWeatherForcast(float lat, float lon, CancellationToken cancellationToken)
        {
            var endpoint = string.Format(configuration.GetSection("WeatherApi:ApiEndPoint").Value!, lat, lon);
            var client = httpClientFactory.CreateClient("WeatherSerivceClient");
            var response = await client.GetAsync(endpoint, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var weatherForcast = await response.Content.ReadFromJsonAsync<WeatherForcast>();
                if (weatherForcast == null) return null;
                var weatherResponse = new WeatherResponse
                {
                    weather = weatherForcast.weather,

                    Dt = DateTimeOffset.FromUnixTimeSeconds(weatherForcast.dt).LocalDateTime,
                    main = weatherForcast.main?.ToMainResponse(),
                    sys = weatherForcast.sys?.ToSysResponse(),
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
    }


}
