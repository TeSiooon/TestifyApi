using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testify.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AuditableEntity : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "UserQuizResults",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "UserQuizResults",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "UserQuizResults",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdatedBy",
            table: "UserQuizResults",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "UserQuizAttempts",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "UserQuizAttempts",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "UserQuizAttempts",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdatedBy",
            table: "UserQuizAttempts",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "UserAnswers",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "UserAnswers",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdatedBy",
            table: "UserAnswers",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Quizzes",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "Quizzes",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Quizzes",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdatedBy",
            table: "Quizzes",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Questions",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "Questions",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Questions",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdatedBy",
            table: "Questions",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Comments",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "Comments",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Comments",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdatedBy",
            table: "Comments",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Answers",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<Guid>(
            name: "CreatedBy",
            table: "Answers",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedAt",
            table: "Answers",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "UpdatedBy",
            table: "Answers",
            type: "uniqueidentifier",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "UserQuizResults");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "UserQuizResults");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "UserQuizResults");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "UserQuizResults");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "UserQuizAttempts");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "UserQuizAttempts");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "UserQuizAttempts");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "UserQuizAttempts");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "UserAnswers");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "UserAnswers");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "UserAnswers");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Quizzes");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "Quizzes");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Quizzes");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "Quizzes");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Comments");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "Comments");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Comments");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "Comments");

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Answers");

        migrationBuilder.DropColumn(
            name: "CreatedBy",
            table: "Answers");

        migrationBuilder.DropColumn(
            name: "UpdatedAt",
            table: "Answers");

        migrationBuilder.DropColumn(
            name: "UpdatedBy",
            table: "Answers");
    }
}
