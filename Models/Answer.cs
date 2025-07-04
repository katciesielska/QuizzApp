namespace QuizzApp.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; } // Indicates if the answer is correct
        public int QuestionId { get; set; } // Foreign key to Question
        public Question Question { get; set; } // Navigation property to Question
    }
}
