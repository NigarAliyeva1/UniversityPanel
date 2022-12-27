using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class AddAppUserColumnToHomeworksTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Homeworks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_AppUserId",
                table: "Homeworks",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homeworks_AspNetUsers_AppUserId",
                table: "Homeworks",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homeworks_AspNetUsers_AppUserId",
                table: "Homeworks");

            migrationBuilder.DropIndex(
                name: "IX_Homeworks_AppUserId",
                table: "Homeworks");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Homeworks");

        }
    }
}
