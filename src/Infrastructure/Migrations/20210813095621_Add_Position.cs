using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrumboard.Infrastructure.Migrations
{
    public partial class Add_Position : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "ListBoards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "ListBoards");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Cards");
        }
    }
}
