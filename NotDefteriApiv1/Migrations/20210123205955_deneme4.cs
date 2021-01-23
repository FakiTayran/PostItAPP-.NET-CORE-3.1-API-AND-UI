using Microsoft.EntityFrameworkCore.Migrations;

namespace NotDefteriApiv1.Migrations
{
    public partial class deneme4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "NotDefteris");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "NotDefteris",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotDefteris");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "NotDefteris",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
