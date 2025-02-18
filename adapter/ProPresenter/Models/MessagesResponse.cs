using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class MessageResponse
    {
        [JsonPropertyName("id")]
        public ApiId Id { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("tokens")]
        public List<MessageToken> Tokens { get; set; } = new List<MessageToken>();

        [JsonPropertyName("theme")]
        public ApiId Theme { get; set; }

        [JsonPropertyName("visible_on_network")]
        public bool VisibleOnNetwork { get; set; }
    }
}
