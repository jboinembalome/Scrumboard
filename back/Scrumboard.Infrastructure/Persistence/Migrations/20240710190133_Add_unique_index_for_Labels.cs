using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrumboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_unique_index_for_Labels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Labels_Id_BoardId",
                table: "Labels",
                columns: new[] { "Id", "BoardId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Labels_Id_BoardId",
                table: "Labels");
        }
    }
}
