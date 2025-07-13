using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;

namespace QuizzApp.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tytuł quizu jest wymagany.")]
        [Display(Name = "Tytuł")]
        [StringLength(200, ErrorMessage = "Tytuł nie może przekraczać 200 znaków.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Opis quizu jest wymagany.")]
        [Display(Name = "Opis")]
        [StringLength(500, ErrorMessage = "Opis nie może przekraczać 500 znaków.")]
        public string Description { get; set; }
     
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public string? UserId { get; set; } // Foreign key to User
        [Display(Name = "Autor")]
        public IdentityUser? User { get; set; } // Navigation property to User
    }
}
