using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010009 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "INFO_ID",
                table: "SL_SHOW",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SL_MOVIE_INFO",
                columns: table => new
                {
                    MOVIE_INFO_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MOVIE_NAME = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MOVIE_OVERVIEW = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TMDB_ID = table.Column<int>(type: "int", nullable: true),
                    OMDB_ID = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OTHER_NAMES = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RUNTIME = table.Column<int>(type: "int", nullable: true),
                    AIR_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LAST_DATA_REFRESH = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LAST_UPDATED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_MOVIE_INFO", x => x.MOVIE_INFO_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SL_TV_INFO",
                columns: table => new
                {
                    TV_INFO_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SHOW_NAME = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SHOW_OVERVIEW = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TMDB_ID = table.Column<int>(type: "int", nullable: true),
                    OMDB_ID = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OTHER_NAMES = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LAST_DATA_REFRESH = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LAST_UPDATED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_TV_INFO", x => x.TV_INFO_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SL_TV_EPISODE_INFO",
                columns: table => new
                {
                    TV_EPISODE_INFO_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TV_EPISODE_ID = table.Column<int>(type: "int", nullable: false),
                    TMDB_ID = table.Column<int>(type: "int", nullable: true),
                    OMDB_ID = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SEASON_NAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EPISODE_NAME = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SEASON_NUMBER = table.Column<int>(type: "int", nullable: true),
                    EPISODE_NUMBER = table.Column<int>(type: "int", nullable: true),
                    EPISODE_OVERVIEW = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RUNTIME = table.Column<int>(type: "int", nullable: true),
                    AIR_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_TV_EPISODE_INFO", x => x.TV_EPISODE_INFO_ID);
                    table.ForeignKey(
                        name: "FK_SL_TV_EPISODE_INFO_SL_TV_INFO_TV_EPISODE_ID",
                        column: x => x.TV_EPISODE_ID,
                        principalTable: "SL_TV_INFO",
                        principalColumn: "TV_INFO_ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SL_TV_EPISODE_INFO_TV_EPISODE_ID",
                table: "SL_TV_EPISODE_INFO",
                column: "TV_EPISODE_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SL_MOVIE_INFO");

            migrationBuilder.DropTable(
                name: "SL_TV_EPISODE_INFO");

            migrationBuilder.DropTable(
                name: "SL_TV_INFO");

            migrationBuilder.DropColumn(
                name: "INFO_ID",
                table: "SL_SHOW");
        }
    }
}
