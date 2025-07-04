using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Constraints;

namespace QuizzApp.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        // Navigation properties
        public ICollection<Question> Questions { get; set; } = new List<Question>();

        public string? UserId { get; set; } // Foreign key to User
        public IdentityUser? User { get; set; } // Navigation property to User
    }
}
