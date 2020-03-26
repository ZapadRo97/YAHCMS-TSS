using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.CulturalService.Migrations
{
    public partial class ChangeLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "locations");
        }
    }
}
