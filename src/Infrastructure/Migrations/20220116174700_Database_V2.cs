using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Scrumboard.Infrastructure.Migrations
{
    public partial class Database_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Boards_BoardId",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Labels_BoardId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Labels");

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Labels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Labels_BoardId",
                table: "Labels",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Boards_BoardId",
                table: "Labels",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
