using System.Text.Json.Serialization;

namespace QuestionnaireApi.Models
{
    public class SubmitQuestionnaireRequest
    {
        public int UserId { get; set; }

        public IEnumerable<QuestionResponse> QuestionResponses { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Department Department { get; set; }
    }
}
