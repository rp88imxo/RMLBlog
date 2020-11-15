using Microsoft.EntityFrameworkCore.Migrations;

namespace RmlBlogMvc.Data.Migrations
{
    public partial class Newuserpropertiesaboutinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserAboutInfoContent",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAboutInfoHeader",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAboutInfoContent",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserAboutInfoHeader",
                table: "AspNetUsers");
        }
    }
}
