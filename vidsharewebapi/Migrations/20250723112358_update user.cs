using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vidsharewebapi.Migrations
{
    /// <inheritdoc />
    public partial class updateuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UserName" },
                values: new object[] { new DateTime(2025, 7, 23, 11, 23, 58, 395, DateTimeKind.Utc).AddTicks(7000), "harshit_k_singh" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 22, 1, 59, 2, 84, DateTimeKind.Utc).AddTicks(4530));
        }
    }
}
