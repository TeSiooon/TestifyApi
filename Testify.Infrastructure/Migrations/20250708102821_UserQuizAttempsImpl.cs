using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testify.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UserQuizAttempsImpl : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "UserQuizAttempts",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                FinishedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserQuizAttempts", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserQuizAttempts_AspNetUsers_UserId",
                    column: x => x.UserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserQuizAttempts_Quizzes_QuizId",
                    column: x => x.QuizId,
                    principalTable: "Quizzes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserAnswers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserQuizAttemptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SelectedAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserAnswers", x => x.Id);
                table.ForeignKey(
                    name: "FK_UserAnswers_Answers_SelectedAnswerId",
                    column: x => x.SelectedAnswerId,
                    principalTable: "Answers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_UserAnswers_Questions_QuestionId",
                    column: x => x.QuestionId,
                    principalTable: "Questions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_UserAnswers_UserQuizAttempts_UserQuizAttemptId",
                    column: x => x.UserQuizAttemptId,
                    principalTable: "UserQuizAttempts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_UserAnswers_QuestionId",
            table: "UserAnswers",
            column: "QuestionId");

        migrationBuilder.CreateIndex(
            name: "IX_UserAnswers_SelectedAnswerId",
            table: "UserAnswers",
            column: "SelectedAnswerId");

        migrationBuilder.CreateIndex(
            name: "IX_UserAnswers_UserQuizAttemptId",
            table: "UserAnswers",
            column: "UserQuizAttemptId");

        migrationBuilder.CreateIndex(
            name: "IX_UserQuizAttempts_QuizId",
            table: "UserQuizAttempts",
            column: "QuizId");

        migrationBuilder.CreateIndex(
            name: "IX_UserQuizAttempts_UserId",
            table: "UserQuizAttempts",
            column: "UserId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "UserAnswers");

        migrationBuilder.DropTable(
            name: "UserQuizAttempts");
    }
}
