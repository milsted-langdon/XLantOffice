using Microsoft.EntityFrameworkCore.Migrations;

namespace XLantDataStore.Migrations
{
    public partial class commissionignore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IgnoreFromCommission",
                table: "MLFSIncome",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IgnoreFromCommission",
                table: "MLFSIncome");
        }
    }
}
