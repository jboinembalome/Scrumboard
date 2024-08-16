using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrumboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_OwnerId_in_Board : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Boards",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Boards");
        }
    }
}
