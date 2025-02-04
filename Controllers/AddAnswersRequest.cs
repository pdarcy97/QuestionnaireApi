using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using QuestionnaireApi.Models;

namespace QuestionnaireApi.Controllers
{
    public class AddAnswersRequest
    {
        public int UserId { get; set; }

        public int QuestionId { get; set; }

        public IEnumerable<Texts> Answers { get; set; }

        [JsonConverter(typeof(StringEnumConverter))] 
        public Department Department { get; set; }
    }
}
