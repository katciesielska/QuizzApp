namespace QuizzApp.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public string? UserId { get; set; } // Foreign key to User
        public int QuestionId { get; set; } // Foreign key to Question
        public int AnswerId { get; set; } // Foreign key to Answer

    }
}
