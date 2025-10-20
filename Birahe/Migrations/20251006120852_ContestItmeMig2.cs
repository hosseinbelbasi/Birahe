using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class ContestItmeMig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 6, 15, 38, 52, 118, DateTimeKind.Local).AddTicks(9912), "9b640de48a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 6, 15, 35, 29, 645, DateTimeKind.Local).AddTicks(8647), "6e359073a3" });
        }
    }
}
