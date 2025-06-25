using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Testify.Domain.Entities;

namespace Testify.Infrastructure.EntityConfigurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(q => q.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(q => q.Description)
            .IsRequired(required: false)
            .HasMaxLength(500);

        builder.Property(q => q.Category)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(q => q.IsPrivate)
            .IsRequired();

        builder.HasMany(q => q.Questions)
            .WithOne(q => q.Quiz)
            .HasForeignKey(q => q.QuizId);

        builder.HasMany(q => q.Comments)
            .WithOne(c => c.Quiz)
            .HasForeignKey(c => c.QuizId);
    }
}
