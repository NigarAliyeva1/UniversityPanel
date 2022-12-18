using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class AddColumnToStaffAndDesignationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesignationId",
                table: "Staff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 17, 22, 49, 28, 201, DateTimeKind.Utc).AddTicks(3066));

            migrationBuilder.CreateIndex(
                name: "IX_Staff_DesignationId",
                table: "Staff",
                column: "DesignationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Designations_DesignationId",
                table: "Staff",
                column: "DesignationId",
                principalTable: "Designations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Designations_DesignationId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Staff_DesignationId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "Staff");

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 17, 22, 45, 30, 960, DateTimeKind.Utc).AddTicks(7501));
        }
    }
}
