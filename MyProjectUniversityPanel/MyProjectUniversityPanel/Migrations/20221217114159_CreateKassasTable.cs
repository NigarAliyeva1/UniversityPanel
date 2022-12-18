using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class CreateKassasTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kassas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<int>(type: "int", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedMoney = table.Column<int>(type: "int", nullable: false),
                    LastModifiedFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeactive = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: DateTime.UtcNow.AddHours(4)),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kassas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kassas_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Kassas",
                columns: new[] { "Id", "AppUserId", "Balance", "IsDeactive", "LastModifiedBy", "LastModifiedFor", "LastModifiedMoney", "LastModifiedTime" },
                values: new object[] { 1, "a4dab9a1-cbf9-4795-a071-b4255ede23d9", 0, false, "", "", 0, new DateTime(2022, 12, 17, 15, 41, 58, 423, DateTimeKind.Utc).AddTicks(8194) });

            migrationBuilder.CreateIndex(
                name: "IX_Kassas_AppUserId",
                table: "Kassas",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kassas");
        }
    }
}
