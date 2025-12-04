using Infrastructure.Weather.Models;
using Infrastructure.Weather.Models.Response;

namespace Infrastructure.Weather.WeatherService
{
    public interface IWeatherServiceMonitor
    {
        Task<WeatherResponse?> GetWeatherForcast(double lat, double lon, CancellationToken cancellationToken);
    }


}
