using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace QuestionnaireApi.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }

        public int OrderNumber { get; set; }

        public Texts Texts { get; set; }

        public ItemType ItemType { get; set; }

        [JsonProperty("questionnaireItems")]
        public IEnumerable<Question> Questions { get; set; }
    }
}
