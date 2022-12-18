using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class CreateSalariesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Money = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false, defaultValue: DateTime.UtcNow.AddHours(4)),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salaries_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Salaries_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 18, 22, 52, 33, 176, DateTimeKind.Utc).AddTicks(1546));

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_AppUserId",
                table: "Salaries",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Salaries_StaffId",
                table: "Salaries",
                column: "StaffId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Salaries");

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 18, 19, 23, 50, 269, DateTimeKind.Utc).AddTicks(7247));
        }
    }
}
