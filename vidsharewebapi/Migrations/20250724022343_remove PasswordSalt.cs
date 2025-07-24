using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vidsharewebapi.Migrations
{
    /// <inheritdoc />
    public partial class removePasswordSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 24, 2, 23, 43, 645, DateTimeKind.Utc).AddTicks(6470));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 7, 23, 15, 15, 41, 810, DateTimeKind.Utc).AddTicks(9250), "salt" });
        }
    }
}
