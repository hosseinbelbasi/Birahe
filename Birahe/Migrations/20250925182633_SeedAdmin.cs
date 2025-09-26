using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Coin", "CreationDateTime", "ModificationDateTime", "Passwordhashed", "RemoveTime", "Role", "SerialNumber", "UserName" },
                values: new object[] { 1, 0, new DateTime(2025, 9, 25, 21, 56, 33, 490, DateTimeKind.Local).AddTicks(4079), null, "fa585d89c851dd338a70dcf535aa2a92fee7836dd6aff1226583e88e0996293f16bc009c652826e0fc5c706695a03cddce372f139eff4d13959da6f1f5d3eabe", null, 0, "4ae94c94-4", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
