using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Testify.Domain.Entities;

namespace Testify.Infrastructure.Persistance;

public class TestifyDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public TestifyDbContext(DbContextOptions<TestifyDbContext> options)
    : base(options)
    {
    }

    //public DbSet<User> Users { get; set; }
    public DbSet<UserQuizResult> UserQuizResults { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Answer> Answers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestifyDbContext).Assembly);
    }
}