using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class MessageTimer
    {
        [JsonPropertyName("configuration")]
        public ClockConfiguration Configuration { get; set; }

        [JsonPropertyName("format")]
        public ClockFormat Format { get; set; }
    }
}
