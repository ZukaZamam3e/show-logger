using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010001Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SL_CODE_VALUE",
                columns: table => new
                {
                    CODE_VALUE_ID = table.Column<int>(type: "int", nullable: false),
                    CODE_TABLE_ID = table.Column<int>(type: "int", nullable: false),
                    DECODE_TXT = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EXTRA_INFO = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_CODE_VALUE", x => x.CODE_VALUE_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SL_SHOW",
                columns: table => new
                {
                    SHOW_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    SHOW_NAME = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SHOW_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    SEASON_NUMBER = table.Column<int>(type: "int", nullable: true),
                    EPISODE_NUMBER = table.Column<int>(type: "int", nullable: true),
                    DATE_WATCHED = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_SHOW", x => x.SHOW_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "SL_CODE_VALUE",
                columns: new[] { "CODE_VALUE_ID", "CODE_TABLE_ID", "DECODE_TXT", "EXTRA_INFO" },
                values: new object[] { 1000, 1, "TV", null });

            migrationBuilder.InsertData(
                table: "SL_CODE_VALUE",
                columns: new[] { "CODE_VALUE_ID", "CODE_TABLE_ID", "DECODE_TXT", "EXTRA_INFO" },
                values: new object[] { 1001, 1, "Movie", null });

            migrationBuilder.InsertData(
                table: "SL_CODE_VALUE",
                columns: new[] { "CODE_VALUE_ID", "CODE_TABLE_ID", "DECODE_TXT", "EXTRA_INFO" },
                values: new object[] { 1002, 1, "AMC", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SL_CODE_VALUE");

            migrationBuilder.DropTable(
                name: "SL_SHOW");
        }
    }
}
