using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testify.Domain.Entities;

namespace Testify.Infrastructure.EntityConfigurations;

public class UserAnswerConfiguration : IEntityTypeConfiguration<UserAnswer>
{
    public void Configure(EntityTypeBuilder<UserAnswer> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.UserQuizAttempt)
               .WithMany(x => x.Answers)
               .HasForeignKey(x => x.UserQuizAttemptId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Question)
               .WithMany()
               .HasForeignKey(x => x.QuestionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SelectedAnswer)
               .WithMany()
               .HasForeignKey(x => x.SelectedAnswerId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
