using Microsoft.AspNetCore.Identity;

namespace QuizzApp.Models
{
    public class QuizAttempt
    {
        public int Id { get; set; }
        public string? UserId { get; set; } 
        public IdentityUser? User { get; set; } 
        public int QuizId { get; set; } 
        public Quiz? Quiz { get; set; } 
        public int Score { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

    }
}
