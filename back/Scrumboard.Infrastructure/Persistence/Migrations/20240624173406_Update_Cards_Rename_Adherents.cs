using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Scrumboard.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Cards_Rename_Adherents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardLabel_Cards_CardsId",
                table: "CardLabel");

            migrationBuilder.DropTable(
                name: "IsMember");

            migrationBuilder.RenameColumn(
                name: "CardsId",
                table: "CardLabel",
                newName: "CardId");

            migrationBuilder.CreateTable(
                name: "CardAssignees",
                columns: table => new
                {
                    AdherentId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardAssignees", x => new { x.AdherentId, x.CardId });
                    table.ForeignKey(
                        name: "FK_CardAssignees_Adherents_AdherentId",
                        column: x => x.AdherentId,
                        principalTable: "Adherents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardAssignees_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardAssignees_CardId",
                table: "CardAssignees",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardLabel_Cards_CardId",
                table: "CardLabel",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardLabel_Cards_CardId",
                table: "CardLabel");

            migrationBuilder.DropTable(
                name: "CardAssignees");

            migrationBuilder.RenameColumn(
                name: "CardId",
                table: "CardLabel",
                newName: "CardsId");

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
                name: "IX_IsMember_CardId",
                table: "IsMember",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardLabel_Cards_CardsId",
                table: "CardLabel",
                column: "CardsId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
