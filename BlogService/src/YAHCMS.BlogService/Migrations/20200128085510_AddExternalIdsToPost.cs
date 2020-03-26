using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.BlogService.Migrations
{
    public partial class AddExternalIdsToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ArtistID",
                table: "posts",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LocationID",
                table: "posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtistID",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "LocationID",
                table: "posts");
        }
    }
}
