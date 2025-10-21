namespace Infrastructure.TimeZone.Models
{
    public class Time_Zone
    {
        public string? name { get; set; }
        public int offset { get; set; }
        public int offset_with_dst { get; set; }
        public string? date { get; set; }
        public string? date_time { get; set; }
        public string? date_time_txt { get; set; }
        public string? date_time_wti { get; set; }
        public string? date_time_ymd { get; set; }
        public float date_time_unix { get; set; }
        public string? time_24 { get; set; }
        public string? time_12 { get; set; }
        public int week { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string? year_abbr { get; set; }
        public string? current_tz_abbreviation { get; set; }
        public string? current_tz_full_name { get; set; }
        public string? standard_tz_abbreviation { get; set; }
        public string? standard_tz_full_name { get; set; }
        public bool is_dst { get; set; }
        public int dst_savings { get; set; }
        public bool dst_exists { get; set; }
        public string? dst_tz_abbreviation { get; set; }
        public string? dst_tz_full_name { get; set; }
        public Dst_Start? dst_start { get; set; }
        public Dst_End? dst_end { get; set; }
    }

}
