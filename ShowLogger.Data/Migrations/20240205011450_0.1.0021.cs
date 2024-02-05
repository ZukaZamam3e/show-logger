using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "API_ID",
                table: "SL_TV_INFO",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "API_ID",
                table: "SL_TV_EPISODE_INFO",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "API_ID",
                table: "SL_MOVIE_INFO",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "API_ID",
                table: "SL_TV_INFO");

            migrationBuilder.DropColumn(
                name: "API_ID",
                table: "SL_TV_EPISODE_INFO");

            migrationBuilder.DropColumn(
                name: "API_ID",
                table: "SL_MOVIE_INFO");
        }
    }
}
