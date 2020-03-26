using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.CulturalService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    PhotoName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "quizes",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quizes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "artists",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    LocationID = table.Column<long>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    DeathDate = table.Column<DateTime>(nullable: false),
                    PhotoName = table.Column<string>(nullable: true),
                    QuizID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.ID);
                    table.ForeignKey(
                        name: "FK_artists_locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_artists_quizes_QuizID",
                        column: x => x.QuizID,
                        principalTable: "quizes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(nullable: true),
                    Answers = table.Column<string>(nullable: true),
                    CorrectAnswer = table.Column<string>(nullable: true),
                    QuizID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_questions_quizes_QuizID",
                        column: x => x.QuizID,
                        principalTable: "quizes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_artists_LocationID",
                table: "artists",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_artists_QuizID",
                table: "artists",
                column: "QuizID");

            migrationBuilder.CreateIndex(
                name: "IX_questions_QuizID",
                table: "questions",
                column: "QuizID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "artists");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "quizes");
        }
    }
}
