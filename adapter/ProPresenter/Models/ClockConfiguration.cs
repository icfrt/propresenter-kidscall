using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class ClockConfiguration
    {
        [JsonPropertyName("id")]
        public ApiId Id { get; set; }

        [JsonPropertyName("allows_overrun")]
        public bool? AllowsOverrun { get; set; }

        [JsonPropertyName("countdown")]
        public CountdownTimer Countdown { get; set; }

        [JsonPropertyName("count_down_to_time")]
        public CountDownToTimeTimer CountDownToTime { get; set; }

        [JsonPropertyName("elapsed")]
        public ElapsedTimer Elapsed { get; set; }
    }
}
