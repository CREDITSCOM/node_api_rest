using Microsoft.EntityFrameworkCore.Migrations;

namespace CS.Db.Migrations
{
    public partial class Init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "RestUser");

            migrationBuilder.AddColumn<string>(
                name: "Test1",
                table: "RestUser",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test1",
                table: "RestUser");

            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "RestUser",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
