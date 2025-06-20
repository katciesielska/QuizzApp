using System.ComponentModel.DataAnnotations;

namespace QuizzApp.Models
{
    public class Option
    {
        [Key]
        public Guid Id { get; set; }
        public required string Text { get; set; }

        public bool IsCorrect { get; set; }

        // FK to the Question
        public Guid QuestionId { get; set; }
        public Question? Question { get; set; } // Navigation property to the Question
    }
}
