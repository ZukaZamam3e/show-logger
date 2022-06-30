using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SL_WATCHLIST",
                columns: table => new
                {
                    WATCHLIST_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    SHOW_NAME = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SHOW_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    SEASON_NUMBER = table.Column<int>(type: "int", nullable: true),
                    EPISODE_NUMBER = table.Column<int>(type: "int", nullable: true),
                    DATE_ADDED = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SHOW_NOTES = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_WATCHLIST", x => x.WATCHLIST_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SL_WATCHLIST");
        }
    }
}
