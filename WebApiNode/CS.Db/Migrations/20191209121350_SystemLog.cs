using Microsoft.EntityFrameworkCore.Migrations;

namespace CS.Db.Migrations
{
    public partial class SystemLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test1",
                table: "RestUser");

            migrationBuilder.CreateTable(
                name: "SystemLog",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlRequest = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLog", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemLog");

            migrationBuilder.AddColumn<string>(
                name: "Test1",
                table: "RestUser",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
