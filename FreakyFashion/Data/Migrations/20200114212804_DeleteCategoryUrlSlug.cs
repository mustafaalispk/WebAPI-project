using Microsoft.EntityFrameworkCore.Migrations;

namespace HemTentan.Data.Migrations
{
    public partial class DeleteCategoryUrlSlug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlSlug",
                table: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlSlug",
                table: "Category",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
