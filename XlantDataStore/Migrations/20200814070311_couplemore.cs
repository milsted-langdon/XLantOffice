using Microsoft.EntityFrameworkCore.Migrations;

namespace XLantDataStore.Migrations
{
    public partial class couplemore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedOtherIncome",
                table: "MLFSSales",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "Locked",
                table: "MLFSReportingPeriods",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedOtherIncome",
                table: "MLFSSales");

            migrationBuilder.DropColumn(
                name: "Locked",
                table: "MLFSReportingPeriods");
        }
    }
}
