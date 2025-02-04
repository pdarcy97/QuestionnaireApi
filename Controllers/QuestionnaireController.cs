using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuestionnaireApi.Models;

namespace QuestionnaireApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionnaireController
    {
        [HttpGet]
        public async Task<Questionnaire?> GetQuestionnaire(int subjectId)
        {
            var questionnaireResponse = await File.ReadAllTextAsync("Data/questionnaire.json", System.Text.Encoding.UTF8);
            
            var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(questionnaireResponse);

            return questionnaire;
        }
    }
}
