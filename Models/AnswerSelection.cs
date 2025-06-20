namespace QuizzApp.Models
{
    public class AnswerSelection
    {
        public Guid Id { get; set; }

        public Guid QuizResultId { get; set; }
        public QuizResult? QuizResult { get; set; }

        public Guid QuestionId { get; set; }
        public Question? Question { get; set; }

        public Guid SelectedOptionId { get; set; }
        public Option? SelectedOption { get; set; }
    }
}
