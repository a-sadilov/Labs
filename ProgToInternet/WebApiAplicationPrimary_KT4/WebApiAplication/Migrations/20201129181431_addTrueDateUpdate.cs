using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiAplication.Migrations
{
    public partial class addTrueDateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "lastUpdateTime",
                table: "LibraryRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastUpdateTime",
                table: "LibraryRecords");
        }
    }
}
