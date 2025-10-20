using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class ContestConfigEndTimeMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "ContestConfigs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ContestConfigs");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BanDateTime", "BanReason", "Coin", "CreationDateTime", "IsBanned", "ModificationDateTime", "Passwordhashed", "RemoveTime", "Role", "SerialNumber", "SolvedRiddles", "TeamName", "Username" },
                values: new object[] { 1000, null, null, 0, new DateTime(2025, 10, 17, 17, 27, 54, 451, DateTimeKind.Local).AddTicks(4997), false, null, "fa585d89c851dd338a70dcf535aa2a92fee7836dd6aff1226583e88e0996293f16bc009c652826e0fc5c706695a03cddce372f139eff4d13959da6f1f5d3eabe", null, 1, "0b1b8a65aa", 0, "hossein", "Admin" });
        }
    }
}
