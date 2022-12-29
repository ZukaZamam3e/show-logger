using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SL_TRANSACTION",
                columns: table => new
                {
                    TRANSACTION_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    USER_ID = table.Column<int>(type: "int", nullable: false),
                    TRANSACTION_TYPE_ID = table.Column<int>(type: "int", nullable: false),
                    SHOW_ID = table.Column<int>(type: "int", nullable: true),
                    ITEM = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    COST_AMT = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DISCOUNT_AMT = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TRANSACTION_NOTES = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TRANSACTION_DATE = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_TRANSACTION", x => x.TRANSACTION_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "SL_CODE_VALUE",
                columns: new[] { "CODE_VALUE_ID", "CODE_TABLE_ID", "DECODE_TXT", "EXTRA_INFO" },
                values: new object[,]
                {
                    { 2000, 2, "A-list Ticket", null },
                    { 2001, 2, "Ticket", null },
                    { 2002, 2, "Purchase", null },
                    { 2003, 2, "AMC A-list", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SL_TRANSACTION");

            migrationBuilder.DeleteData(
                table: "SL_CODE_VALUE",
                keyColumn: "CODE_VALUE_ID",
                keyValue: 2000);

            migrationBuilder.DeleteData(
                table: "SL_CODE_VALUE",
                keyColumn: "CODE_VALUE_ID",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "SL_CODE_VALUE",
                keyColumn: "CODE_VALUE_ID",
                keyValue: 2002);

            migrationBuilder.DeleteData(
                table: "SL_CODE_VALUE",
                keyColumn: "CODE_VALUE_ID",
                keyValue: 2003);
        }
    }
}
