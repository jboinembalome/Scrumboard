using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrumboard.Infrastructure.Migrations
{
    public partial class Add_IsMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adherents_Cards_CardId",
                table: "Adherents");

            migrationBuilder.DropForeignKey(
                name: "FK_Adherents_Teams_TeamId",
                table: "Adherents");

            migrationBuilder.DropIndex(
                name: "IX_Adherents_CardId",
                table: "Adherents");

            migrationBuilder.DropIndex(
                name: "IX_Adherents_TeamId",
                table: "Adherents");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "Adherents");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Adherents");

            migrationBuilder.CreateTable(
                name: "AdherentTeam",
                columns: table => new
                {
                    AdherentsId = table.Column<int>(type: "int", nullable: false),
                    TeamsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdherentTeam", x => new { x.AdherentsId, x.TeamsId });
                    table.ForeignKey(
                        name: "FK_AdherentTeam_Adherents_AdherentsId",
                        column: x => x.AdherentsId,
                        principalTable: "Adherents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdherentTeam_Teams_TeamsId",
                        column: x => x.TeamsId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IsMember",
                columns: table => new
                {
                    AdherentId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IsMember", x => new { x.AdherentId, x.CardId });
                    table.ForeignKey(
                        name: "FK_IsMember_Adherents_AdherentId",
                        column: x => x.AdherentId,
                        principalTable: "Adherents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IsMember_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdherentTeam_TeamsId",
                table: "AdherentTeam",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_IsMember_CardId",
                table: "IsMember",
                column: "CardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdherentTeam");

            migrationBuilder.DropTable(
                name: "IsMember");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "Adherents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Adherents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adherents_CardId",
                table: "Adherents",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Adherents_TeamId",
                table: "Adherents",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adherents_Cards_CardId",
                table: "Adherents",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adherents_Teams_TeamId",
                table: "Adherents",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
