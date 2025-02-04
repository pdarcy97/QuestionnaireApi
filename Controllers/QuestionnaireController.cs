using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuestionnaireApi.Models;
using QuestionnaireApi.Service;

namespace QuestionnaireApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionnaireController(ResponseService responseService)
    {
        [HttpGet]
        public async Task<Questionnaire?> GetQuestionnaire()
        {
            var questionnaireResponse = await File.ReadAllTextAsync("Data/questionnaire.json", System.Text.Encoding.UTF8);
            var questionnaireFromFile = JsonConvert.DeserializeObject<Questionnaire>(questionnaireResponse);

            return questionnaireFromFile;
        }

        [HttpGet]
        [Route("{subjectId}")]
        public async Task<IActionResult> GetQuestionsForSubject(int subjectId, int pageNumber = 1, int pageSize = 1)
        {
            var questionnaireResponse = await File.ReadAllTextAsync("Data/questionnaire.json", System.Text.Encoding.UTF8);
            
            var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(questionnaireResponse);

            if (questionnaire == null) return new BadRequestObjectResult("No questionnaire file found");
            var subject = questionnaire.Subjects.FirstOrDefault(s => s.SubjectId == subjectId);

            if (subject == null) return new BadRequestObjectResult("Subject not found");

            var paginatedQuestions = subject.Questions.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new OkObjectResult(paginatedQuestions);
        }

        [HttpPost]
        public Task<IActionResult> SubmitQuestionnaire([FromBody] SubmitQuestionnaireRequest request)
        {
            responseService.AddResponse(
                new QuestionnaireResponse
                {
                    UserId = request.UserId,
                    Department = request.Department,
                    QuestionResponses = request.QuestionResponses,
                    OverallScore = CalculateOverallScore(request.QuestionResponses)
                }
            );

            var newResults = responseService.GetResponses();

            return Task.FromResult<IActionResult>(new OkObjectResult(newResults));

            int CalculateOverallScore(IEnumerable<QuestionResponse> questionResponses)
            {
                var overallScore = 0;
                foreach (var questionResponse in questionResponses)
                {
                    overallScore += questionResponse.Score;
                }

                return overallScore;
            }

        }

        [HttpGet]
        [Route("results")]
        public async Task<IActionResult> GetResults()
        {
            var questionnaireFile = await File.ReadAllTextAsync("Data/questionnaire.json", System.Text.Encoding.UTF8);
            var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(questionnaireFile);

            var results = InitializeQuestionScores(questionnaire);

            var responses = responseService.GetResponses();

            foreach (var questionnaireResponse in responses)
            {
                foreach (var questionResponse in questionnaireResponse.QuestionResponses)
                {
                    var result = results.FirstOrDefault(r => r.QuestionId == questionResponse.QuestionId);

                    if (result == null) continue;
                    if (questionResponse.Score < int.MaxValue)
                    {
                        result.Min = questionResponse.Score;
                    }

                    if (questionResponse.Score > result.Max)
                    {
                        result.Max = questionResponse.Score;
                    }

                    result.TotalScore += questionResponse.Score;
                    result.Responses++;
                }
            }

            return new OkObjectResult(results);
        }

        private static IEnumerable<QuestionScores> InitializeQuestionScores(Questionnaire questionnaire)
        {
            List<QuestionScores> questionScores = [];
            questionScores.AddRange(from subject in questionnaire.Subjects
            from question in subject.Questions
            select new QuestionScores
            {
                QuestionId = question.QuestionId,
                Max = 0,
                Min = 0,
                Responses = 0,
                TotalScore = 0,
            });

            return questionScores;
        }
    }
}
