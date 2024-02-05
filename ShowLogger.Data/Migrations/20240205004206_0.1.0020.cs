using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "API_TYPE",
                table: "SL_TV_INFO",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "API_TYPE",
                table: "SL_TV_EPISODE_INFO",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "API_TYPE",
                table: "SL_MOVIE_INFO",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "API_TYPE",
                table: "SL_TV_INFO");

            migrationBuilder.DropColumn(
                name: "API_TYPE",
                table: "SL_TV_EPISODE_INFO");

            migrationBuilder.DropColumn(
                name: "API_TYPE",
                table: "SL_MOVIE_INFO");
        }
    }
}
