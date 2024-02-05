using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OMDB_ID",
                table: "SL_TV_INFO");

            migrationBuilder.DropColumn(
                name: "TMDB_ID",
                table: "SL_TV_INFO");

            migrationBuilder.DropColumn(
                name: "OMDB_ID",
                table: "SL_TV_EPISODE_INFO");

            migrationBuilder.DropColumn(
                name: "TMDB_ID",
                table: "SL_TV_EPISODE_INFO");

            migrationBuilder.DropColumn(
                name: "OMDB_ID",
                table: "SL_MOVIE_INFO");

            migrationBuilder.DropColumn(
                name: "TMDB_ID",
                table: "SL_MOVIE_INFO");

            migrationBuilder.AlterColumn<string>(
                name: "API_ID",
                table: "SL_TV_INFO",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "API_ID",
                table: "SL_TV_EPISODE_INFO",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "API_ID",
                table: "SL_MOVIE_INFO",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "API_ID",
                table: "SL_TV_INFO",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OMDB_ID",
                table: "SL_TV_INFO",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TMDB_ID",
                table: "SL_TV_INFO",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "API_ID",
                table: "SL_TV_EPISODE_INFO",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OMDB_ID",
                table: "SL_TV_EPISODE_INFO",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TMDB_ID",
                table: "SL_TV_EPISODE_INFO",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "API_ID",
                table: "SL_MOVIE_INFO",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(25)",
                oldMaxLength: 25,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "OMDB_ID",
                table: "SL_MOVIE_INFO",
                type: "varchar(25)",
                maxLength: 25,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TMDB_ID",
                table: "SL_MOVIE_INFO",
                type: "int",
                nullable: true);
        }
    }
}
