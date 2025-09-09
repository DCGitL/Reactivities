namespace Infrastructure.Weather.Models
{
    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string? country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }

}
