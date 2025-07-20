using Microsoft.AspNetCore.Identity;

namespace QuizzApp.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int QuizAttemptId { get; set; } 
        public QuizAttempt? QuizAttempt { get; set; } 
        public int QuestionId { get; set; } 
        public Question? Question { get; set; } 
        public int AnswerId { get; set; }
        public Answer? Answer { get; set; } 
        public string? UserId { get; set; } 
        public IdentityUser? User { get; set; } 
    }
}
