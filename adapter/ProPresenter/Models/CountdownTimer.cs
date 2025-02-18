using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class CountdownTimer
    {
        [JsonPropertyName("duration")]
        public int? Duration { get; set; }
    }
}
