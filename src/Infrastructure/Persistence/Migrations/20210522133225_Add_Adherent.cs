using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrumboard.Infrastructure.Persistence.Migrations
{
    public partial class Add_Adherent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Adherent_AdherentId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Adherent_Cards_CardId",
                table: "Adherent");

            migrationBuilder.DropForeignKey(
                name: "FK_Adherent_Teams_TeamId",
                table: "Adherent");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Adherent_AdherentId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Adherent_AdherentId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adherent",
                table: "Adherent");

            migrationBuilder.RenameTable(
                name: "Adherent",
                newName: "Adherents");

            migrationBuilder.RenameIndex(
                name: "IX_Adherent_TeamId",
                table: "Adherents",
                newName: "IX_Adherents_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Adherent_CardId",
                table: "Adherents",
                newName: "IX_Adherents_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adherents",
                table: "Adherents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Adherents_AdherentId",
                table: "Activities",
                column: "AdherentId",
                principalTable: "Adherents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Adherents_AdherentId",
                table: "Boards",
                column: "AdherentId",
                principalTable: "Adherents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Adherents_AdherentId",
                table: "Comments",
                column: "AdherentId",
                principalTable: "Adherents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Adherents_AdherentId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Adherents_Cards_CardId",
                table: "Adherents");

            migrationBuilder.DropForeignKey(
                name: "FK_Adherents_Teams_TeamId",
                table: "Adherents");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Adherents_AdherentId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Adherents_AdherentId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Adherents",
                table: "Adherents");

            migrationBuilder.RenameTable(
                name: "Adherents",
                newName: "Adherent");

            migrationBuilder.RenameIndex(
                name: "IX_Adherents_TeamId",
                table: "Adherent",
                newName: "IX_Adherent_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Adherents_CardId",
                table: "Adherent",
                newName: "IX_Adherent_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Adherent",
                table: "Adherent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Adherent_AdherentId",
                table: "Activities",
                column: "AdherentId",
                principalTable: "Adherent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adherent_Cards_CardId",
                table: "Adherent",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adherent_Teams_TeamId",
                table: "Adherent",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Adherent_AdherentId",
                table: "Boards",
                column: "AdherentId",
                principalTable: "Adherent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Adherent_AdherentId",
                table: "Comments",
                column: "AdherentId",
                principalTable: "Adherent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
