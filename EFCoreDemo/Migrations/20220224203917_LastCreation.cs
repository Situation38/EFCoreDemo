using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCoreDemo.Migrations
{
    public partial class LastCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "EmailAddress");

            migrationBuilder.AddColumn<string>(
                name: "Mail",
                table: "EmailAddress",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mail",
                table: "EmailAddress");

            migrationBuilder.AddColumn<int>(
                name: "Name",
                table: "EmailAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
