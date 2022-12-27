using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyProjectUniversityPanel.Migrations
{
    public partial class AddExamsColumnToStudentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 24, 0, 23, 36, 968, DateTimeKind.Utc).AddTicks(965));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Kassas",
                keyColumn: "Id",
                keyValue: 1,
                column: "LastModifiedTime",
                value: new DateTime(2022, 12, 23, 23, 51, 26, 275, DateTimeKind.Utc).AddTicks(1125));
        }
    }
}
