using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.CulturalService.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_quizes_QuizID",
                table: "questions");

            migrationBuilder.AlterColumn<long>(
                name: "QuizID",
                table: "questions",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_questions_quizes_QuizID",
                table: "questions",
                column: "QuizID",
                principalTable: "quizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_questions_quizes_QuizID",
                table: "questions");

            migrationBuilder.AlterColumn<long>(
                name: "QuizID",
                table: "questions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_questions_quizes_QuizID",
                table: "questions",
                column: "QuizID",
                principalTable: "quizes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
