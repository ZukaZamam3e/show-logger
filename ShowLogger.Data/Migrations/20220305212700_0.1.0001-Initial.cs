using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010001Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SL_SHOW_SHOW_ID_SEQ",
                startValue: 1000L);

            migrationBuilder.CreateTable(
                name: "SL_CODE_VALUE",
                columns: table => new
                {
                    CODE_VALUE_ID = table.Column<int>(type: "int", nullable: false),
                    CODE_TABLE_ID = table.Column<int>(type: "int", nullable: false),
                    DECODE_TXT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EXTRA_INFO = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_CODE_VALUE", x => x.CODE_VALUE_ID);
                });

            migrationBuilder.CreateTable(
                name: "SL_SHOW",
                columns: table => new
                {
                    SHOW_ID = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR SL_SHOW_SHOW_ID_SEQ"),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    SHOW_NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SHOW_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    SEASON_NUMBER = table.Column<int>(type: "int", nullable: true),
                    EPISODE_NUMBER = table.Column<int>(type: "int", nullable: true),
                    DateWatched = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_SHOW", x => x.SHOW_ID);
                });

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

            migrationBuilder.DropSequence(
                name: "SL_SHOW_SHOW_ID_SEQ");
        }
    }
}
