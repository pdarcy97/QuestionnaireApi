using QuestionnaireApi.Models;

namespace QuestionnaireApi.Service
{
    public class ResponseService
    {
        private List<QuestionnaireResponse> questionnaireResponses = [];

        public void AddResponse(QuestionnaireResponse questionnaireResponse)
        {
            questionnaireResponses.Add(questionnaireResponse);
        }

        public List<QuestionnaireResponse> GetResponses()
        {
            return questionnaireResponses;
        }
    }
}
