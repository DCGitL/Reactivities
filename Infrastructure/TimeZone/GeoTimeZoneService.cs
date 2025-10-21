using System;
using System.Net.Http.Json;
using API.Helper;
using Infrastructure.TimeZone.Models;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.TimeZone;

public class GeoTimeZoneService : IGeoTimeZoneService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IConfiguration _configuration;

    public GeoTimeZoneService(IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _configuration = configuration;
    }

    public async Task<TimeZoneResponse?> GetTimeZoneResponseAsync(float lat, float lon)
    {
        var client = _clientFactory.CreateClient(HttpClientName.TimezoneServiceClient);
        var endpoint = $"{_configuration.GetSection("Timezone:ApiEndPoint").Value}&lat={lat}&long={lon}";  //"/v2/timezone? apiKey = 56e6788ddcd64f888acda21ee2161451 & lat = 51.5074 & long = -0.1278"
        var response = await client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TimeZoneResponse>();

            return result;
        }
        return null;

    }

}
