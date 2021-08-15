using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrumboard.Infrastructure.Migrations
{
    public partial class Add_IsPinned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPinned",
                table: "Boards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPinned",
                table: "Boards");
        }
    }
}
