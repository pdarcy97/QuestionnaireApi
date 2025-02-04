using Newtonsoft.Json;

namespace QuestionnaireApi.Models
{
    public class Questionnaire
    {
        public int QuestionnaireId { get; set; }

        [JsonProperty("questionnaireItems")]
        public IEnumerable<Subject> Subjects { get; set; }
    }
}
