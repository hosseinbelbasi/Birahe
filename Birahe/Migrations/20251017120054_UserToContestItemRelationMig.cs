using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class UserToContestItemRelationMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 17, 15, 30, 53, 826, DateTimeKind.Local).AddTicks(1358), "cdaef4a39e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 17, 14, 47, 36, 209, DateTimeKind.Local).AddTicks(489), "6ff8826ce4" });
        }
    }
}
