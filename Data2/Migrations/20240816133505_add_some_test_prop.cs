using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class add_some_test_prop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestTime",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "UserInfo",
                table: "TestResults");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TestResults");

            migrationBuilder.AddColumn<string>(
                name: "UserInfo",
                table: "TestResults",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
