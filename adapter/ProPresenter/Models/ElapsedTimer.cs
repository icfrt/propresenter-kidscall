using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class ElapsedTimer
    {
        [JsonPropertyName("start_time")]
        public int? StartTime { get; set; }
    }
}
