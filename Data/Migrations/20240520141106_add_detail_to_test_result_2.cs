using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class add_detail_to_test_result_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Detail",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Detail",
                table: "TestResults");
        }
    }
}
