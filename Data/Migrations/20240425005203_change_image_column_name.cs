using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class change_image_column_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "QuestionSets",
                newName: "ImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "QuestionSets",
                newName: "Image");
        }
    }
}
