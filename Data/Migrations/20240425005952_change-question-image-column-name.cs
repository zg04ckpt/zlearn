using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class changequestionimagecolumnname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "QuestionSets",
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
                table: "QuestionSets",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Questions",
                newName: "Image");
        }
    }
}
