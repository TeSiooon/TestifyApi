using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testify.Domain.Entities;

namespace Testify.Infrastructure.EntityConfigurations;

public class UserQuizResultConfiguration : IEntityTypeConfiguration<UserQuizResult>
{
    public void Configure(EntityTypeBuilder<UserQuizResult> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Score)
            .IsRequired();

        builder.Property(u => u.Attempts)
            .IsRequired();

        builder.Property(u => u.CompletedDate)
            .IsRequired();

        builder.HasOne(u => u.User)
            .WithMany()
            .HasForeignKey(u => u.UserId);

        builder.HasOne(u => u.Quiz)
            .WithMany()
            .HasForeignKey(u => u.QuizId);
    }
}
