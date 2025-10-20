using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class ImageFileNameMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 13, 16, 17, 55, 360, DateTimeKind.Local).AddTicks(8388), "a1e5b4e2c4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 12, 11, 21, 42, 989, DateTimeKind.Local).AddTicks(2209), "76a2f43dfa" });
        }
    }
}
