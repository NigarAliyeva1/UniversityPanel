using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class CreateIncomesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Money = table.Column<int>(type: "int", nullable: false),
                    For = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: DateTime.UtcNow.AddHours(4)),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incomes_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 17, 15, 44, 50, 784, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_AppUserId",
                table: "Incomes",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 17, 15, 41, 58, 423, DateTimeKind.Utc).AddTicks(8194));
        }
    }
}
