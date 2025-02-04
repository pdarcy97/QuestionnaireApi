using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireApi.Controllers;
using QuestionnaireApi.Models;
using QuestionnaireApi.Service;

namespace QuestionnaireApiTests
{
    public class QuestionnaireControllerTests
    {
        private readonly ResponseService responseService;
        private readonly QuestionnaireController _sut;

        public QuestionnaireControllerTests()
        {
            responseService = new ResponseService();
            _sut = new QuestionnaireController(responseService);
        }

        [Fact]
        public async Task GetQuestionsForSubject()
        {
            var result = await _sut.GetQuestionsForSubject(2605515, 1, 3) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal((result.Value as List<Question>).Count, 3);
        }

        [Fact]
        public async Task GetQuestionsForSubject_On_Correct_Page()
        {
            var result = await _sut.GetQuestionsForSubject(2605515, 2, 1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal((result.Value as List<Question>)[0].QuestionId, 3807643);
            Assert.Equal((result.Value as List<Question>).Count, 1);
        }

        [Fact]
        public async Task SubmitQuestionnaire_Should_Save_New_Response()
        {
            var request = new SubmitQuestionnaireRequest
            {
                UserId = 2334,
                Department = Department.Development,
                QuestionResponses =
                [
                    new QuestionResponse
                    {
                        QuestionId = 3807638,
                        Score = 3
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807643,
                        Score = 5
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851855,
                        Score = 4
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807701,
                        Score = 1
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807644,
                        Score = 1
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851843,
                        Score = 2
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851856,
                        Score = 3
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3810105,
                        Score = 5
                    },
                ]
            };

            await _sut.SubmitQuestionnaire(request);
            var result = responseService.GetResponses();

            Assert.Equal(result[0].UserId, 2334);
            Assert.Equal(result[0].OverallScore, 24);
            Assert.Equal(result[0].Department, Department.Development);
        }

        [Fact]
        public async Task GetResults_Should_Return_Min_Max_And_Average_Scores_For_Each_Question()
        {
            var firstRequest = new SubmitQuestionnaireRequest
            {
                UserId = 2334,
                Department = Department.Development,
                QuestionResponses =
                [
                    new QuestionResponse
                    {
                        QuestionId = 3807638,
                        Score = 3
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807643,
                        Score = 5
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851855,
                        Score = 4
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807701,
                        Score = 1
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807644,
                        Score = 1
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851843,
                        Score = 2
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851856,
                        Score = 3
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3810105,
                        Score = 5
                    },
                ]
            };
            await _sut.SubmitQuestionnaire(firstRequest);
            var secondRequest = new SubmitQuestionnaireRequest
            {
                UserId = 1832,
                Department = Department.Reception,
                QuestionResponses =
                [
                    new QuestionResponse
                    {
                        QuestionId = 3807638,
                        Score = 1
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807643,
                        Score = 1
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851855,
                        Score = 2
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807701,
                        Score = 3
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3807644,
                        Score = 4
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851843,
                        Score = 1
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3851856,
                        Score = 5
                    },
                    new QuestionResponse
                    {
                        QuestionId = 3810105,
                        Score = 5
                    },
                ]
            };
            await _sut.SubmitQuestionnaire(secondRequest);

            var results = await _sut.GetResults() as OkObjectResult;
            
            Assert.NotNull(results.Value);
            Assert.Equal((results.Value as List<QuestionScores>).Count, 8);
            Assert.Equal((results.Value as List<QuestionScores>)[0].Min, 1);
            Assert.Equal((results.Value as List<QuestionScores>)[0].Max, 3);
            Assert.Equal((results.Value as List<QuestionScores>)[0].Average, 2);
        }
    }
}