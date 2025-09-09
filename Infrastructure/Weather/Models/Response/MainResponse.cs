namespace Infrastructure.Weather.Models
{
    public class MainResponse
    {
        public float temp { get; set; }
        public float feels_like { get; set; }
        public float temp_min { get; set; }
        public float temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public int sea_level { get; set; }
        public int grnd_level { get; set; }
    }

    public static class MainResponseExtensions
    {
        public static MainResponse ToMainResponse(this Main main)
        {
            return new MainResponse
            {
                temp = main.temp,
                feels_like = main.feels_like,
                temp_min = main.temp_min,
                temp_max = main.temp_max,
                pressure = main.pressure,
                humidity = main.humidity,
                sea_level = main.sea_level,
                grnd_level = main.grnd_level
            };
        }
    }

}
