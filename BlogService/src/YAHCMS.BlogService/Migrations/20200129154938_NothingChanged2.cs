using Microsoft.EntityFrameworkCore.Migrations;

namespace YAHCMS.BlogService.Migrations
{
    public partial class NothingChanged2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_blogs_BlogID",
                table: "posts");

            migrationBuilder.AlterColumn<long>(
                name: "BlogID",
                table: "posts",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_blogs_BlogID",
                table: "posts",
                column: "BlogID",
                principalTable: "blogs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_blogs_BlogID",
                table: "posts");

            migrationBuilder.AlterColumn<long>(
                name: "BlogID",
                table: "posts",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_posts_blogs_BlogID",
                table: "posts",
                column: "BlogID",
                principalTable: "blogs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
