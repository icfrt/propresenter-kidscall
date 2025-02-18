using System.Text.Json.Serialization;

namespace Icf.ProPresenter.KidsCall.ProPresenter.Models
{
    public class CountDownToTimeTimer
    {
        [JsonPropertyName("time_of_day")]
        public string TimeOfDay { get; set; }

        [JsonPropertyName("period")]
        public string Period { get; set; }
    }



}
