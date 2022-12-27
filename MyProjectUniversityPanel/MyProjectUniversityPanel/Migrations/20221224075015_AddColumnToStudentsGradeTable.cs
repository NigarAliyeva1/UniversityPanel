using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class AddColumnToStudentsGradeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeactive",
                table: "StudentGrades",
                newName: "IsQuiz");

            migrationBuilder.AddColumn<bool>(
                name: "IsExam",
                table: "StudentGrades",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMidterm",
                table: "StudentGrades",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPresentation",
                table: "StudentGrades",
                type: "bit",
                nullable: false,
                defaultValue: false);

        
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExam",
                table: "StudentGrades");

            migrationBuilder.DropColumn(
                name: "IsMidterm",
                table: "StudentGrades");

            migrationBuilder.DropColumn(
                name: "IsPresentation",
                table: "StudentGrades");

            migrationBuilder.RenameColumn(
                name: "IsQuiz",
                table: "StudentGrades",
                newName: "IsDeactive");
 
        }
    }
}
