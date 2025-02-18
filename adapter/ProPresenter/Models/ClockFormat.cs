using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class ClockFormat
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("is_24_hours")]
        public bool? Is24Hours { get; set; }

        [JsonPropertyName("hour")]
        public string Hour { get; set; }

        [JsonPropertyName("minute")]
        public string Minute { get; set; }

        [JsonPropertyName("second")]
        public string Second { get; set; }

        [JsonPropertyName("millisecond")]
        public string Millisecond { get; set; }
    }



}
