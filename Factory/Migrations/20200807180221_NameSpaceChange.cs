using Microsoft.EntityFrameworkCore.Migrations;

namespace Factory.Migrations
{
    public partial class NameSpaceChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Engineers",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Engineers",
                newName: "Type");
        }
    }
}
