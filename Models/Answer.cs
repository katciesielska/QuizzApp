using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace QuizzApp.Models
{
    public class Answer
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Treść odpowiedzi jest wymagana.")]
        [Display(Name = "Treść odpowiedzi")]
        [StringLength(200, ErrorMessage = "Treść odpowiedzi nie może przekraczać 200 znaków.")]
        public string? Text { get; set; }
        [Display(Name = "Czy poprawna?")]
        public bool IsCorrect { get; set; } // Indicates if the answer is correct
        public int QuestionId { get; set; } // Foreign key to Question
        public Question? Question { get; set; } // Navigation property to Question
        public string? UserId { get; set; } // Foreign key to User
        public IdentityUser? User { get; set; } // Navigation property to User
    }
}
