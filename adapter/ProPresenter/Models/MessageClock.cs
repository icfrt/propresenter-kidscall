using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class MessageClock
    {
        [JsonPropertyName("format")]
        public ClockFormat Format { get; set; }
    }
}
