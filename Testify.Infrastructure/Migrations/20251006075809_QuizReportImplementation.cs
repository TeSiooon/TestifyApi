using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testify.Infrastructure.Migrations;

/// <inheritdoc />
public partial class QuizReportImplementation : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "QuizReport",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ReportedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_QuizReport", x => x.Id);
                table.ForeignKey(
                    name: "FK_QuizReport_AspNetUsers_ReportedById",
                    column: x => x.ReportedById,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_QuizReport_Quizzes_QuizId",
                    column: x => x.QuizId,
                    principalTable: "Quizzes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_QuizReport_QuizId",
            table: "QuizReport",
            column: "QuizId");

        migrationBuilder.CreateIndex(
            name: "IX_QuizReport_ReportedById",
            table: "QuizReport",
            column: "ReportedById");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "QuizReport");
    }
}
