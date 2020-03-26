using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.CulturalService.Migrations
{
    public partial class ArtistTypeWithData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "artists");

            migrationBuilder.AddColumn<long>(
                name: "TypeID",
                table: "artists",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "artistTypes",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artistTypes", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "artistTypes",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[] { -1L, "Handy with words", "Poet" });

            migrationBuilder.InsertData(
                table: "artistTypes",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[] { -2L, "Handy with a brush", "Painter" });

            migrationBuilder.InsertData(
                table: "artistTypes",
                columns: new[] { "ID", "Description", "Name" },
                values: new object[] { -3L, "An artist who makes sculptures", "Sculptor" });

            migrationBuilder.CreateIndex(
                name: "IX_artists_TypeID",
                table: "artists",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_artists_artistTypes_TypeID",
                table: "artists",
                column: "TypeID",
                principalTable: "artistTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_artists_artistTypes_TypeID",
                table: "artists");

            migrationBuilder.DropTable(
                name: "artistTypes");

            migrationBuilder.DropIndex(
                name: "IX_artists_TypeID",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "TypeID",
                table: "artists");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "artists",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
