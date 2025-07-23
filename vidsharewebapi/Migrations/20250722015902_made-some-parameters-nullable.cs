using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vidsharewebapi.Migrations
{
    /// <inheritdoc />
    public partial class madesomeparametersnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KeyId",
                table: "VideoInfos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl720",
                table: "VideoDownloadUrls",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl480",
                table: "VideoDownloadUrls",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl1080",
                table: "VideoDownloadUrls",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 22, 1, 59, 2, 84, DateTimeKind.Utc).AddTicks(4530));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VideoInfos",
                keyColumn: "KeyId",
                keyValue: null,
                column: "KeyId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "KeyId",
                table: "VideoInfos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "VideoDownloadUrls",
                keyColumn: "DownloadUrl720",
                keyValue: null,
                column: "DownloadUrl720",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl720",
                table: "VideoDownloadUrls",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "VideoDownloadUrls",
                keyColumn: "DownloadUrl480",
                keyValue: null,
                column: "DownloadUrl480",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl480",
                table: "VideoDownloadUrls",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "VideoDownloadUrls",
                keyColumn: "DownloadUrl1080",
                keyValue: null,
                column: "DownloadUrl1080",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DownloadUrl1080",
                table: "VideoDownloadUrls",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 21, 15, 15, 24, 818, DateTimeKind.Utc).AddTicks(6630));
        }
    }
}
