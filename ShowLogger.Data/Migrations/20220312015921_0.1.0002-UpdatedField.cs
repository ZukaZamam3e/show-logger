using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShowLogger.Data.Migrations
{
    public partial class _010002UpdatedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateWatched",
                table: "SL_SHOW",
                newName: "DATE_WATCHED");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DATE_WATCHED",
                table: "SL_SHOW",
                newName: "DateWatched");
        }
    }
}
