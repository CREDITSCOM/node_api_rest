using Microsoft.EntityFrameworkCore.Migrations;

namespace CS.Db.Migrations
{
    public partial class add_stats_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Key);
                });

            migrationBuilder.InsertData(
                table: "Statistics",
                columns: new[] { "Key", "Description", "Value" },
                values: new object[] { "supply", "Total amount of Credits coins supplied", "249'471'071" });

            migrationBuilder.InsertData(
                table: "Statistics",
                columns: new[] { "Key", "Description", "Value" },
                values: new object[] { "active", "Amount of Credits coins in active circulation", "249'171'071" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statistics");
        }
    }
}
