using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroVerse.Data.Migrations
{
    public partial class FollowerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    FollowingUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FollowedUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserModelEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => new { x.FollowingUserId, x.FollowedUserId });
                    table.ForeignKey(
                        name: "FK_Follows_UserModel_UserModelEmail",
                        column: x => x.UserModelEmail,
                        principalTable: "UserModel",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Follows_UserModelEmail",
                table: "Follows",
                column: "UserModelEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Follows");

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
    }
}
