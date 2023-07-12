using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroVerse.Data.Migrations
{
    public partial class ActivationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "AspNetUsers",
                newName: "Activation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activation",
                table: "AspNetUsers",
                newName: "Role");
        }
    }
}
