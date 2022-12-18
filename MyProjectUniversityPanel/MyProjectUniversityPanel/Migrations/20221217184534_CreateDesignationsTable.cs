using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class CreateDesignationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "Staff");

            migrationBuilder.AddColumn<int>(
                name: "Salary",
                table: "Staff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Designations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Designations", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 17, 22, 45, 30, 960, DateTimeKind.Utc).AddTicks(7501));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Designations");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Staff");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 17, 20, 58, 44, 806, DateTimeKind.Utc).AddTicks(343));
        }
    }
}
