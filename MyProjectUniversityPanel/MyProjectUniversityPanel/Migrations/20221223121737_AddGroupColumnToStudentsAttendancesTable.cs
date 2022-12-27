using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class AddGroupColumnToStudentsAttendancesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "StudentsAttendances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 23, 16, 17, 36, 725, DateTimeKind.Utc).AddTicks(5830));

            migrationBuilder.CreateIndex(
                name: "IX_StudentsAttendances_GroupId",
                table: "StudentsAttendances",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsAttendances_Groups_GroupId",
                table: "StudentsAttendances",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsAttendances_Groups_GroupId",
                table: "StudentsAttendances");

            migrationBuilder.DropIndex(
                name: "IX_StudentsAttendances_GroupId",
                table: "StudentsAttendances");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "StudentsAttendances");

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 23, 1, 4, 26, 465, DateTimeKind.Utc).AddTicks(8795));
        }
    }
}
