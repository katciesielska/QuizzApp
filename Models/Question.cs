using System.ComponentModel.DataAnnotations;

namespace QuizzApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Treść pytania jest wymagana.")]
        [Display(Name = "Treść pytania")]
        [StringLength(300, ErrorMessage = "Treść pytania nie może przekraczać 300 znaków.")]
        public string Text { get; set; }
        public int QuizId { get; set; } // Foreign key to Quiz
        public Quiz? Quiz { get; set; } // Navigation property to Quiz
        [Range(1, int.MaxValue, ErrorMessage ="Liczba punktów musi być liczbą dodatnią.")]
        public int Score { get; set; } // Points awarded for answering the question correctly
        public ICollection<Answer> Answers { get; set; } = new List<Answer>(); // Collection of answers for the question

    }
}
