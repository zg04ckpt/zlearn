using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class updateuserpropertities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_AppUsers_UserId",
                table: "TestResults");

            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Tests_TestId",
                table: "TestResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_AppUsers_AuthorId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_AuthorId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_TestId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_UserId",
                table: "TestResults");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Tests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfAttempts",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfQuestions",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "TestResults",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TestName",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_AppUserId",
                table: "Tests",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_AppUserId",
                table: "TestResults",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_AppUsers_AppUserId",
                table: "TestResults",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_AppUsers_AppUserId",
                table: "Tests",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_AppUsers_AppUserId",
                table: "TestResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_AppUsers_AppUserId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_AppUserId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_AppUserId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "NumberOfAttempts",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "NumberOfQuestions",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "TestName",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TestResults");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorId",
                table: "Tests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_AuthorId",
                table: "Tests",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestId",
                table: "TestResults",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_UserId",
                table: "TestResults",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_AppUsers_UserId",
                table: "TestResults",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Tests_TestId",
                table: "TestResults",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_AppUsers_AuthorId",
                table: "Tests",
                column: "AuthorId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
