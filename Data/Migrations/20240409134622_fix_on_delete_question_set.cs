﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ZG04WEB.Data.Migrations
{
    public partial class fix_on_delete_question_set : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionSets_QuestionSetId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionSets_QuestionSetId",
                table: "Questions",
                column: "QuestionSetId",
                principalTable: "QuestionSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionSets_QuestionSetId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionSets_QuestionSetId",
                table: "Questions",
                column: "QuestionSetId",
                principalTable: "QuestionSets",
                principalColumn: "Id");
        }
    }
}
