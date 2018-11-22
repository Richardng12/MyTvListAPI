using Microsoft.EntityFrameworkCore.Migrations;

namespace mytvlistapi.Migrations
{
    public partial class newCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Authentication",
                table: "TvItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Authentication",
                table: "TvItem");
        }
    }
}
