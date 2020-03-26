using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.CulturalService.Migrations
{
    public partial class ArtistModelChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_artists_locations_LocationID",
                table: "artists");

            migrationBuilder.DropForeignKey(
                name: "FK_artists_artistTypes_TypeID",
                table: "artists");

            migrationBuilder.DropIndex(
                name: "IX_artists_LocationID",
                table: "artists");

            migrationBuilder.DropIndex(
                name: "IX_artists_TypeID",
                table: "artists");

            migrationBuilder.AlterColumn<long>(
                name: "TypeID",
                table: "artists",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "LocationID",
                table: "artists",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeathDate",
                table: "artists",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TypeID",
                table: "artists",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "LocationID",
                table: "artists",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeathDate",
                table: "artists",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_artists_LocationID",
                table: "artists",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_artists_TypeID",
                table: "artists",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_artists_locations_LocationID",
                table: "artists",
                column: "LocationID",
                principalTable: "locations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_artists_artistTypes_TypeID",
                table: "artists",
                column: "TypeID",
                principalTable: "artistTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
