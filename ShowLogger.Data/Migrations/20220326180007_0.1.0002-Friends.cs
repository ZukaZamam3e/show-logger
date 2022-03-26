using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010002Friends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SHOW_NOTES",
                table: "SL_SHOW",
                type: "varchar(250)",
                maxLength: 250,
                nullable: true,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SL_FRIEND",
                columns: table => new
                {
                    FRIEND_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    FRIEND_USER_ID = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_FRIEND", x => x.FRIEND_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SL_FRIEND_REQUEST",
                columns: table => new
                {
                    FRIEND_REQUEST_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SENT_USER_ID = table.Column<int>(type: "int", nullable: false),
                    RECEIVED_USER_ID = table.Column<int>(type: "int", nullable: false),
                    DATE_SENT = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_FRIEND_REQUEST", x => x.FRIEND_REQUEST_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SL_FRIEND");

            migrationBuilder.DropTable(
                name: "SL_FRIEND_REQUEST");

            migrationBuilder.DropColumn(
                name: "SHOW_NOTES",
                table: "SL_SHOW");
        }
    }
}
