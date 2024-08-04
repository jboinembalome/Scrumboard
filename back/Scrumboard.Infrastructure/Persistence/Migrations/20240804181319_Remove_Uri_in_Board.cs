using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrumboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Uri_in_Board : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uri",
                table: "Boards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Uri",
                table: "Boards",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
