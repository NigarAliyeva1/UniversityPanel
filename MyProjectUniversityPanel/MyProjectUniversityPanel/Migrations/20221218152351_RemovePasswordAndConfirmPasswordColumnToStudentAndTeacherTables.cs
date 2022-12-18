using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class RemovePasswordAndConfirmPasswordColumnToStudentAndTeacherTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Students");

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 18, 19, 23, 50, 269, DateTimeKind.Utc).AddTicks(7247));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 18, 0, 42, 31, 682, DateTimeKind.Utc).AddTicks(8194));
        }
    }
}
