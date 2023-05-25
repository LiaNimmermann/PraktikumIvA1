using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroVerse.Data.Migrations
{
    public partial class ActivationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Activation",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activation",
                table: "AspNetUsers");
        }
    }
}
