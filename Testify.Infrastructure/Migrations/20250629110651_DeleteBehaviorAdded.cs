using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testify.Infrastructure.Migrations;

/// <inheritdoc />
public partial class DeleteBehaviorAdded : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Answers_Questions_QuestionId",
            table: "Answers");

        migrationBuilder.AddForeignKey(
            name: "FK_Answers_Questions_QuestionId",
            table: "Answers",
            column: "QuestionId",
            principalTable: "Questions",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Answers_Questions_QuestionId",
            table: "Answers");

        migrationBuilder.AddForeignKey(
            name: "FK_Answers_Questions_QuestionId",
            table: "Answers",
            column: "QuestionId",
            principalTable: "Questions",
            principalColumn: "Id");
    }
}
