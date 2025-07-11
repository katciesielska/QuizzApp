using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizzApp.Models;

namespace QuizzApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<QuizzApp.Models.Quiz> Quiz { get; set; } = default!;
        public DbSet<QuizzApp.Models.Question> Question { get; set; } = default!;
    }
}
