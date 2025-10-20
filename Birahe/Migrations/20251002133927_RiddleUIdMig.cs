using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class RiddleUIdMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "RiddleUId",
                table: "Riddles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Coin", "CreationDateTime", "ModificationDateTime", "Passwordhashed", "RemoveTime", "Role", "SerialNumber", "Username" },
                values: new object[] { 100, 0, new DateTime(2025, 10, 2, 17, 9, 26, 213, DateTimeKind.Local).AddTicks(5108), null, "fa585d89c851dd338a70dcf535aa2a92fee7836dd6aff1226583e88e0996293f16bc009c652826e0fc5c706695a03cddce372f139eff4d13959da6f1f5d3eabe", null, 1, "68e2afabfb", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DropColumn(
                name: "RiddleUId",
                table: "Riddles");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Coin", "CreationDateTime", "ModificationDateTime", "Passwordhashed", "RemoveTime", "Role", "SerialNumber", "Username" },
                values: new object[] { 1, 0, new DateTime(2025, 9, 26, 14, 14, 8, 162, DateTimeKind.Local).AddTicks(5002), null, "fa585d89c851dd338a70dcf535aa2a92fee7836dd6aff1226583e88e0996293f16bc009c652826e0fc5c706695a03cddce372f139eff4d13959da6f1f5d3eabe", null, 1, "d18768c709", "Admin" });
        }
    }
}
