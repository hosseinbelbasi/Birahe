using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class TeamNameMig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamName",
                table: "Users",
                column: "TeamName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_TeamName",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BanDateTime", "BanReason", "Coin", "CreationDateTime", "IsBanned", "ModificationDateTime", "Passwordhashed", "RemoveTime", "Role", "SerialNumber", "TeamName", "Username" },
                values: new object[] { 1000, null, null, 0, new DateTime(2025, 10, 17, 14, 44, 47, 179, DateTimeKind.Local).AddTicks(4463), false, null, "fa585d89c851dd338a70dcf535aa2a92fee7836dd6aff1226583e88e0996293f16bc009c652826e0fc5c706695a03cddce372f139eff4d13959da6f1f5d3eabe", null, 1, "9aae0ce5b7", "hossein", "Admin" });
        }
    }
}
