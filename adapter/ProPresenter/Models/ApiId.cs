using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class ApiId
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("uuid")]
        public string? Uuid { get; set; }
    }
}
