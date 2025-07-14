namespace QuizzApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int QuizId { get; set; } // Foreign key to Quiz
        public Quiz? Quiz { get; set; } // Navigation property to Quiz
        public int Score { get; set; } // Points awarded for answering the question correctly
        public ICollection<Answer> Answers { get; set; } = new List<Answer>(); // Collection of answers for the question

    }
}
