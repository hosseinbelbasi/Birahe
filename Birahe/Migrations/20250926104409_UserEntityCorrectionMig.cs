using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class UserEntityCorrectionMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDateTime", "Role", "SerialNumber", "Username" },
                values: new object[] { new DateTime(2025, 9, 26, 14, 14, 8, 162, DateTimeKind.Local).AddTicks(5002), 1, "d18768c709", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationDateTime", "Role", "SerialNumber", "Username" },
                values: new object[] { new DateTime(2025, 9, 25, 21, 56, 33, 490, DateTimeKind.Local).AddTicks(4079), 0, "4ae94c94-4", null });
        }
    }
}
