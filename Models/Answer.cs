namespace QuestionnaireApi.Models
{
    public class Answer
    {
        public int? AnswerId { get; set; }

        public int QuestionId { get; set; }

        public int AnswerType { get; set; }

        public int OrderNumber { get; set; }

        public Texts Texts { get; set; }

        public int ItemType { get; set; }
    }
}
