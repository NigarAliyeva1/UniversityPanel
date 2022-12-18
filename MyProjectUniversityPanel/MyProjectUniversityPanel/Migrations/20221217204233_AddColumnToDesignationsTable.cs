using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class AddColumnToDesignationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeactive",
                table: "Designations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 18, 0, 42, 31, 682, DateTimeKind.Utc).AddTicks(8194));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeactive",
                table: "Designations");

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 17, 22, 49, 28, 201, DateTimeKind.Utc).AddTicks(3066));
        }
    }
}
