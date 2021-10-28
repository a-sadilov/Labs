using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiAplication.Migrations
{
    public partial class deleteSomeProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "LibraryRecords");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "LibraryRecords");

            migrationBuilder.DropColumn(
                name: "YearOfIssue",
                table: "LibraryRecords");

            migrationBuilder.RenameColumn(
                name: "lastUpdateTime",
                table: "LibraryRecords",
                newName: "LastUpdateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdateTime",
                table: "LibraryRecords",
                newName: "lastUpdateTime");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "LibraryRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "LibraryRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearOfIssue",
                table: "LibraryRecords",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
