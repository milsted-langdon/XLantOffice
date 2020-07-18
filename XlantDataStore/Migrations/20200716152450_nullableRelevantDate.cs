using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XLantDataStore.Migrations
{
    public partial class nullableRelevantDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RelevantDate",
                table: "MLFSIncome",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RelevantDate",
                table: "MLFSIncome",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
