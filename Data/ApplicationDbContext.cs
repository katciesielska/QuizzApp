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

        public DbSet<Models.Question> Questions { get; set; }
        public DbSet<Models.Option> Options { get; set; }
        public DbSet<Models.Quiz> Quizzes { get; set; }
        public DbSet<Models.QuizResult> QuizResults { get; set; }
        public DbSet<Models.AnswerSelection> AnswerSelections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // QuizResult → Quiz
            builder.Entity<QuizResult>()
                .HasOne(qr => qr.Quiz)
                .WithMany(q => q.Results)
                .HasForeignKey(qr => qr.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuizResult → User
            builder.Entity<QuizResult>()
                .HasOne(qr => qr.User)
                .WithMany(u => u.QuizResults)
                .HasForeignKey(qr => qr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Question → Quiz
            builder.Entity<Question>()
                .HasOne(q => q.Quiz)
                .WithMany(qz => qz.Questions)
                .HasForeignKey(q => q.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            // AnswerSelection → QuizResult
            builder.Entity<AnswerSelection>()
                .HasOne(a => a.QuizResult)
                .WithMany(qr => qr.Answers)
                .HasForeignKey(a => a.QuizResultId)
                .OnDelete(DeleteBehavior.Restrict);

            // AnswerSelection → Question
            builder.Entity<AnswerSelection>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // AnswerSelection → Option
            builder.Entity<AnswerSelection>()
                .HasOne(a => a.SelectedOption)
                .WithMany()
                .HasForeignKey(a => a.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }

}
