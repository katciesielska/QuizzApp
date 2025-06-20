namespace QuizzApp.Models
{
    public class QuizResult
    {
        public Guid Id { get; set; }

        public Guid QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int Score { get; set; }
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        public List<AnswerSelection> Answers { get; set; }
    }
}
