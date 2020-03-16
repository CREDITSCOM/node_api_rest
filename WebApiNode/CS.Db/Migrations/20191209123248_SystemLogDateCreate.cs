using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CS.Db.Migrations
{
    public partial class SystemLogDateCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreate",
                table: "SystemLog",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_SystemLog_DateCreate",
                table: "SystemLog",
                column: "DateCreate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SystemLog_DateCreate",
                table: "SystemLog");

            migrationBuilder.DropColumn(
                name: "DateCreate",
                table: "SystemLog");
        }
    }
}
