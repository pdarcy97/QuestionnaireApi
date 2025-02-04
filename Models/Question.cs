using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace QuestionnaireApi.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public int SubjectId { get; set; }

        public int AnswerCategoryType { get; set; }

        public int OrderNumber { get; set; }

        public Texts Texts { get; set; }

        public ItemType ItemType { get; set; }

        [JsonProperty(PropertyName = "questionnaireItems")]
        public IEnumerable<Answer> Answers { get; set; }
    }
}
