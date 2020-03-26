using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.CulturalService.Migrations
{
    public partial class ArtistFKAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ArtistTypeID",
                table: "artists",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_artists_ArtistTypeID",
                table: "artists",
                column: "ArtistTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_artists_LocationID",
                table: "artists",
                column: "LocationID");

            migrationBuilder.AddForeignKey(
                name: "FK_artists_artistTypes_ArtistTypeID",
                table: "artists",
                column: "ArtistTypeID",
                principalTable: "artistTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_artists_locations_LocationID",
                table: "artists",
                column: "LocationID",
                principalTable: "locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_artists_artistTypes_ArtistTypeID",
                table: "artists");

            migrationBuilder.DropForeignKey(
                name: "FK_artists_locations_LocationID",
                table: "artists");

            migrationBuilder.DropIndex(
                name: "IX_artists_ArtistTypeID",
                table: "artists");

            migrationBuilder.DropIndex(
                name: "IX_artists_LocationID",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "ArtistTypeID",
                table: "artists");
        }
    }
}
