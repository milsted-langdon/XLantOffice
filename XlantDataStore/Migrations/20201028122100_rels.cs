using Microsoft.EntityFrameworkCore.Migrations;

namespace XLantDataStore.Migrations
{
    public partial class rels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelatedClients",
                table: "MLFSSales",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedClients",
                table: "MLFSSales");
        }
    }
}
