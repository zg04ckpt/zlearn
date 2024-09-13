using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class adduserintest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_AppUsers_AppUserId",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_AppUserId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "TestResults");

            migrationBuilder.CreateTable(
                name: "SavedTest",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MarkedAt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInTests", x => new { x.UserId, x.TestId });
                    table.ForeignKey(
                        name: "FK_UserInTests_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserInTests_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInTests_TestId",
                table: "SavedTest",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedTest");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "TestResults",
                type: "uniqueidentifier",
                nullable: true);

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
        }
    }
}
