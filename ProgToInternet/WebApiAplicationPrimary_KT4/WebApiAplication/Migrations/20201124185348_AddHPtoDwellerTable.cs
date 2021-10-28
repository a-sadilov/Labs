using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiAplication.Migrations
{
    public partial class AddHPtoDwellerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HP",
                table: "Dwellers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HP",
                table: "Dwellers");
        }
    }
}
