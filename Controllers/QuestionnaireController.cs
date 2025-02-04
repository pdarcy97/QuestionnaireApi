﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetQuestionsForSubject(int subjectId)
        {
            var questionnaireResponse = await File.ReadAllTextAsync("Data/questionnaire.json", System.Text.Encoding.UTF8);
            
            var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(questionnaireResponse);

            if (questionnaire == null) return new BadRequestObjectResult("No questionnaire file found");
            var subject = questionnaire.Subjects.FirstOrDefault(s => s.SubjectId == subjectId);

            if (subject == null) return new BadRequestObjectResult("No questionnaire file found");
            const int pageNumber = 1;
            const int pageSize = 10;

            var paginatedQuestions = subject.Questions.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new OkObjectResult(paginatedQuestions);

        }

        //[HttpPost]
        //public async Task<IActionResult> AddAnswerToQuestion([FromBody] AddAnswersRequest request)
        //{
        //    var questionnaireResponse = await File.ReadAllTextAsync("Data/questionnaire.json", System.Text.Encoding.UTF8);
        //    var questionnaire = JsonConvert.DeserializeObject<Questionnaire>(questionnaireResponse);

        //    if (questionnaire == null) return new BadRequestObjectResult("No questionnaire file found");

        //    foreach (var subject in questionnaire.Subjects)
        //    {
        //        foreach (var question in subject.Questions)
        //        {
        //            if (question.QuestionId != request.QuestionId) continue;
                    
        //            var i = 0;
        //            foreach (var value in request.Answers)
        //            {
        //                var answer = new { i = i++, value };
        //                question.Answers = question.Answers.Concat([
        //                    new Answer
        //                    {
        //                        AnswerId = answer.i,
        //                        AnswerType = AnswerType.Option,
        //                        ItemType = ItemType.Answer,
        //                        OrderNumber = answer.i,
        //                        QuestionId = request.QuestionId,
        //                        Texts = answer.value
        //                    }
        //                ]);
        //            }
        //        }
        //    }

        //    await File.WriteAllTextAsync("Data/questionnaire.json", JsonConvert.SerializeObject(questionnaire));
        //    return new OkObjectResult(questionnaire);
        //}

        [HttpPost]
        public async Task<IActionResult> SubmitQuestionnaire([FromBody] SubmitQuestionnaireRequest request)
        {
            responseService.AddResponse(
                new QuestionnaireResponse
                {
                    Department = request.Department,
                    UserId = request.UserId,
                    QuestionResponses = request.QuestionResponses,
                    OverallScore = CalculateOverallScore(request.QuestionResponses)
                }
            );

            var newResults = responseService.GetResponses();

            return new OkObjectResult(newResults);

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

            foreach (var questionnaireResponse in responses.QuestionnaireResponses)
            {
                foreach (var questionResponse in questionnaireResponse.QuestionResponses)
                {
                    var result = results.FirstOrDefault(r => r.QuestionId == questionResponse.QuestionId);

                    if (result == null) continue;
                    if (questionResponse.Score < result.Min)
                    {
                        result.Min = questionResponse.Score;
                    }

                    if (questionResponse.Score > result.Max)
                    {
                        result.Max = questionResponse.Score;
                    }

                    result.TotalScore += questionResponse.Score;
                }
            }

            return new OkObjectResult(results);
        }

        private IEnumerable<QuestionScores> InitializeQuestionScores(Questionnaire questionnaire)
        {
            List<QuestionScores> questionScores = [];

            foreach (var subject in questionnaire.Subjects)
            {
                foreach (var question in subject.Questions)
                {
                    questionScores.Add(new QuestionScores
                    {
                        QuestionId = question.QuestionId,
                        Max = 0,
                        Min = 0,
                        Responses = 0,
                        TotalScore = 0,
                    });
                }
            }

            return questionScores;
        }
    }
}
