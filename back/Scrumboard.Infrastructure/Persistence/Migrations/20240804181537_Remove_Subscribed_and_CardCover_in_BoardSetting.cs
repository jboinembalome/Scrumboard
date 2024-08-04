using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrumboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Subscribed_and_CardCover_in_BoardSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardCoverImage",
                table: "BoardSettings");

            migrationBuilder.DropColumn(
                name: "Subscribed",
                table: "BoardSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CardCoverImage",
                table: "BoardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Subscribed",
                table: "BoardSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
