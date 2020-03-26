using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.CulturalService.Migrations
{
    public partial class RemoveArtistFromQuiz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_artists_quizes_QuizID",
                table: "artists");

            migrationBuilder.DropIndex(
                name: "IX_artists_QuizID",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "QuizID",
                table: "artists");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "QuizID",
                table: "artists",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_artists_QuizID",
                table: "artists",
                column: "QuizID");

            migrationBuilder.AddForeignKey(
                name: "FK_artists_quizes_QuizID",
                table: "artists",
                column: "QuizID",
                principalTable: "quizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
