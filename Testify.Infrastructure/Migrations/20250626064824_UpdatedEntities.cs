using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testify.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UpdatedEntities : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "MyProperty",
            table: "Questions");

        migrationBuilder.DropColumn(
            name: "Order",
            table: "Questions");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "MyProperty",
            table: "Questions",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "Order",
            table: "Questions",
            type: "int",
            nullable: false,
            defaultValue: 0);
    }
}
