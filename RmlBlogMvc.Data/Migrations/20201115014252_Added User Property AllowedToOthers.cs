using Microsoft.EntityFrameworkCore.Migrations;

namespace RmlBlogMvc.Data.Migrations
{
    public partial class AddedUserPropertyAllowedToOthers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserAboutInfoAllowedToOthers",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAboutInfoAllowedToOthers",
                table: "AspNetUsers");
        }
    }
}
