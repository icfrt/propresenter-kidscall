using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class MessageText
    {
        [JsonPropertyName("text")]
        public string? Text { get; set; }

        public static implicit operator MessageText(string? s) => new MessageText { Text = s };
    }
}
