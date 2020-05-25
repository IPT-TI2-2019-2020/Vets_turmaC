using Microsoft.EntityFrameworkCore.Migrations;

namespace ClinicaVet.Data.Migrations
{
    public partial class AssociarVetrinariosAutenticacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Veterinarios",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Veterinarios");
        }
    }
}
