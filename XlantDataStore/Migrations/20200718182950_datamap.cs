using Microsoft.EntityFrameworkCore.Migrations;

namespace XLantDataStore.Migrations
{
    public partial class datamap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataMaps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(nullable: true),
                    SourceName = table.Column<string>(nullable: true),
                    InternalFieldName = table.Column<string>(nullable: true),
                    ExternalFieldName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataMaps", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataMaps");
        }
    }
}
