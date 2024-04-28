using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SL_TV_EPISODE_ORDER",
                columns: table => new
                {
                    TV_EPISODE_ORDER_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TV_INFO_ID = table.Column<int>(type: "int", nullable: false),
                    TV_EPISODE_INFO_ID = table.Column<int>(type: "int", nullable: false),
                    EPISODE_ORDER = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SL_TV_EPISODE_ORDER", x => x.TV_EPISODE_ORDER_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SL_TV_EPISODE_ORDER");
        }
    }
}
