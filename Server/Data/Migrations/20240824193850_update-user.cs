using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class updateuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_AppUsers_AppUserId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_AppUserId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Tests");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_AppUsers_AuthorId",
                table: "Tests",
                column: "AuthorId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_AppUsers_AuthorId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_AuthorId",
                table: "Tests");

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

            migrationBuilder.CreateIndex(
                name: "IX_Tests_AppUserId",
                table: "Tests",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_AppUsers_AppUserId",
                table: "Tests",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
