namespace Infrastructure.TimeZone.Models
{
    public class Dst_End
    {
        public string? utc_time { get; set; }
        public string? duration { get; set; }
        public bool gap { get; set; }
        public string? date_time_after { get; set; }
        public string? date_time_before { get; set; }
        public bool overlap { get; set; }
    }

}
