using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class MessageToken
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("text")]
        public MessageText? Text { get; set; }

        [JsonPropertyName("clock")]
        public MessageClock? Clock { get; set; }

        [JsonPropertyName("timer")]
        public MessageTimer? Timer { get; set; }
    }
}
