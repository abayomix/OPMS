﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repo_PMS.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_departments",
                table: "departments");

            migrationBuilder.RenameTable(
                name: "departments",
                newName: "Department");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AssesmentResponses",
                columns: table => new
                {
                    ARID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssementID = table.Column<int>(type: "int", nullable: false),
                    QID = table.Column<int>(type: "int", nullable: false),
                    AnswerResponse = table.Column<int>(type: "int", nullable: false),
                    iscorrect = table.Column<bool>(type: "bit", nullable: false),
                    Empid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createddate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentResponses", x => x.ARID);
                });

            migrationBuilder.CreateTable(
                name: "AssesmentReviewRatios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssesmentRatio = table.Column<int>(type: "int", nullable: false),
                    ReviewRatio = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssesmentReviewRatios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "assesments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssessmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AssessmentYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AssessmentCAtegory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    A_Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assesments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBack_Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBack_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FQID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    score = table.Column<int>(type: "int", nullable: false),
                    empid = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBY = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedbacks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Qid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question_Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option_1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option_2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option_3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option_4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correct = table.Column<int>(type: "int", nullable: false),
                    DeptID = table.Column<int>(type: "int", nullable: false),
                    DegID = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Qid);
                });

            migrationBuilder.CreateTable(
                name: "QuestionSets",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssesmentID = table.Column<int>(type: "int", nullable: false),
                    AssesmentYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssesmentCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dept = table.Column<int>(type: "int", nullable: false),
                    Deg = table.Column<int>(type: "int", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBY = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReviewName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReviewYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    review_Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssesmentResponses");

            migrationBuilder.DropTable(
                name: "AssesmentReviewRatios");

            migrationBuilder.DropTable(
                name: "assesments");

            migrationBuilder.DropTable(
                name: "FeedBack_Questions");

            migrationBuilder.DropTable(
                name: "feedbacks");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionSets");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "departments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_departments",
                table: "departments",
                column: "Id");
        }
    }
}
