using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DDStudy2022.DAL.Migrations
{
    public partial class UpdateUserAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "UserAccounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AvatarId",
                table: "UserAccounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
