using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QuestionnaireApi.Models
{
    public class Answer
    {
        public int? AnswerId { get; set; }

        public int QuestionId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] 
        public AnswerType AnswerType { get; set; }

        public int OrderNumber { get; set; }

        public Texts Texts { get; set; }

        public ItemType ItemType { get; set; }
    }
}
