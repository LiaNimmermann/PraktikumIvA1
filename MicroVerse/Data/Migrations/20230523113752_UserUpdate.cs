using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroVerse.Data.Migrations
{
    public partial class UserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserModelEmail",
                table: "UserModel",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserModel_UserModelEmail",
                table: "UserModel",
                column: "UserModelEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_UserModel_UserModel_UserModelEmail",
                table: "UserModel",
                column: "UserModelEmail",
                principalTable: "UserModel",
                principalColumn: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserModel_UserModel_UserModelEmail",
                table: "UserModel");

            migrationBuilder.DropIndex(
                name: "IX_UserModel_UserModelEmail",
                table: "UserModel");

            migrationBuilder.DropColumn(
                name: "UserModelEmail",
                table: "UserModel");
        }
    }
}
