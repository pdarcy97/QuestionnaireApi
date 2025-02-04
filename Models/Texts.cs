using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace QuestionnaireApi.Models
{
    public class Texts
    {
        [JsonProperty("nl-NL")]
        public string Dutch { get; set; }

        [JsonProperty("en-US")]
        public string English { get; set; }
    }
}
