using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.Web
{
    class EntryModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("timeStamp")]
        public DateTime TimeStamp { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}
