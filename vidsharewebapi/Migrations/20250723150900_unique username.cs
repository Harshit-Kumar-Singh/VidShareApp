using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vidsharewebapi.Migrations
{
    /// <inheritdoc />
    public partial class uniqueusername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 15, 9, 0, 668, DateTimeKind.Utc).AddTicks(3830));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 23, 14, 53, 29, 220, DateTimeKind.Utc).AddTicks(1860));
        }
    }
}
