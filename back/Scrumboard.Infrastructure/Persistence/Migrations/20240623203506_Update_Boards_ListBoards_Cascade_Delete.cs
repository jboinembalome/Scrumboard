using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrumboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Boards_ListBoards_Cascade_Delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListBoards_Boards_BoardId",
                table: "ListBoards");

            migrationBuilder.AddForeignKey(
                name: "FK_ListBoards_Boards_BoardId",
                table: "ListBoards",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListBoards_Boards_BoardId",
                table: "ListBoards");

            migrationBuilder.AddForeignKey(
                name: "FK_ListBoards_Boards_BoardId",
                table: "ListBoards",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");
        }
    }
}
