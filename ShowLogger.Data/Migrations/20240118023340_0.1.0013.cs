using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SL_TV_EPISODE_INFO_SL_TV_INFO_TV_EPISODE_ID",
                table: "SL_TV_EPISODE_INFO");

            migrationBuilder.RenameColumn(
                name: "TV_EPISODE_ID",
                table: "SL_TV_EPISODE_INFO",
                newName: "TV_INFO_ID");

            migrationBuilder.RenameIndex(
                name: "IX_SL_TV_EPISODE_INFO_TV_EPISODE_ID",
                table: "SL_TV_EPISODE_INFO",
                newName: "IX_SL_TV_EPISODE_INFO_TV_INFO_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SL_TV_EPISODE_INFO_SL_TV_INFO_TV_INFO_ID",
                table: "SL_TV_EPISODE_INFO",
                column: "TV_INFO_ID",
                principalTable: "SL_TV_INFO",
                principalColumn: "TV_INFO_ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SL_TV_EPISODE_INFO_SL_TV_INFO_TV_INFO_ID",
                table: "SL_TV_EPISODE_INFO");

            migrationBuilder.RenameColumn(
                name: "TV_INFO_ID",
                table: "SL_TV_EPISODE_INFO",
                newName: "TV_EPISODE_ID");

            migrationBuilder.RenameIndex(
                name: "IX_SL_TV_EPISODE_INFO_TV_INFO_ID",
                table: "SL_TV_EPISODE_INFO",
                newName: "IX_SL_TV_EPISODE_INFO_TV_EPISODE_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_SL_TV_EPISODE_INFO_SL_TV_INFO_TV_EPISODE_ID",
                table: "SL_TV_EPISODE_INFO",
                column: "TV_EPISODE_ID",
                principalTable: "SL_TV_INFO",
                principalColumn: "TV_INFO_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
