using Microsoft.EntityFrameworkCore.Migrations;

namespace mytvlistapi.Migrations
{
    public partial class newCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "TvItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "TvItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "TvItem");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "TvItem");
        }
    }
}
