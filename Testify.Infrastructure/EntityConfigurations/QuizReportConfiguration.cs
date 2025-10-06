using Microsoft.EntityFrameworkCore;
using Testify.Domain.Entities;

namespace Testify.Infrastructure.EntityConfigurations;

public class QuizReportConfiguration : IEntityTypeConfiguration<QuizReport>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<QuizReport> builder)
    {
        builder.HasKey(qr => qr.Id);

        builder.Property(qr => qr.Reason)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(qr => qr.Quiz)
            .WithMany(q => q.Reports)
            .HasForeignKey(qr => qr.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(qr => qr.ReportedBy)
            .WithMany(u => u.QuizReports)
            .HasForeignKey(qr => qr.ReportedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
