﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<QuizzApp.Models.Answer> Answer { get; set; } = default!;
        public DbSet<QuizzApp.Models.UserAnswer> UserAnswer { get; set; } = default!;
        public DbSet<QuizzApp.Models.QuizAttempt> QuizAttempt { get; set; } = default!;
    }
}
