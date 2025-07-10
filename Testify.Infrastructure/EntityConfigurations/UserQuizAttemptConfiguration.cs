using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testify.Domain.Entities;

namespace Testify.Infrastructure.EntityConfigurations;

public class UserQuizAttemptConfiguration : IEntityTypeConfiguration<UserQuizAttempt>
{
    public void Configure(EntityTypeBuilder<UserQuizAttempt> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.User)
            .WithMany(u => u.QuizAttempts)
            .HasForeignKey(a => a.UserId)
            .IsRequired();

        builder.HasOne(a => a.Quiz)
            .WithMany()
            .HasForeignKey(a => a.QuizId)
            .IsRequired();
    }
}
