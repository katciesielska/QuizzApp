using System.ComponentModel.DataAnnotations.Schema;

namespace QuizzApp.Models
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string? Description { get; set; }

        // FK to user
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public List<Question> Questions { get; set; }
        public List<QuizResult> Results { get; set; }

    }

}

