using Microsoft.EntityFrameworkCore.Migrations;

namespace HemTentan.Data.Migrations
{
    public partial class AddRoleAdministrator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5c7fe465-38b3-4cf0-8514-a28de513e3ed", "caebc253-8aeb-469a-bc5f-3376613172d8", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c7fe465-38b3-4cf0-8514-a28de513e3ed");
        }
    }
}
