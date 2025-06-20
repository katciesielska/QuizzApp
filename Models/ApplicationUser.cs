using Microsoft.AspNetCore.Identity;

namespace QuizzApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Quiz> Quizzes { get; set; } = new();
        public List<QuizResult> QuizResults { get; set; } = new();
    }
}
