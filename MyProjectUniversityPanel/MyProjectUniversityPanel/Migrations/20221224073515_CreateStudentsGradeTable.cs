using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class CreateStudentsGradeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exam",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Exam1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Exam2",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Exam3",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Exam4",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Midterm",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Midterm1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Midterm2",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Midterm3",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Midterm4",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Presentation",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Presentation1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Presentation2",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Presentation3",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Presentation4",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Quiz",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Quiz1",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Quiz2",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Quiz3",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Quiz4",
                table: "Students");

            migrationBuilder.CreateTable(
                name: "StudentGrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Midterm = table.Column<int>(type: "int", nullable: false),
                    Quiz = table.Column<int>(type: "int", nullable: false),
                    Presentation = table.Column<int>(type: "int", nullable: false),
                    Exam = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeactive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGrades_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentGrades_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentGrades_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_AppUserId",
                table: "StudentGrades",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_StudentId",
                table: "StudentGrades",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentGrades_TeacherId",
                table: "StudentGrades",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentGrades");

            migrationBuilder.AddColumn<int>(
                name: "Exam",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Exam1",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Exam2",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Exam3",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Exam4",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Midterm",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Midterm1",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Midterm2",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Midterm3",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Midterm4",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Presentation",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Presentation1",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Presentation2",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Presentation3",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Presentation4",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quiz",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quiz1",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quiz2",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quiz3",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quiz4",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

          
        }
    }
}
