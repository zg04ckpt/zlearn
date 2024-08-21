using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class changequestionimagecolumnname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Tests",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Questions",
                newName: "ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Tests",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Questions",
                newName: "Image");
        }
    }
}
