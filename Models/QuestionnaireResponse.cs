namespace QuestionnaireApi.Models
{
    public class QuestionnaireResponse
    {
        public int UserId { get; set; }

        public Department Department { get; set; }

        public IEnumerable<QuestionResponse> QuestionResponses { get; set; }

        public int OverallScore { get; set; }
    }
}
