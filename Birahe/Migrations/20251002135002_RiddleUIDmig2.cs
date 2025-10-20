using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class RiddleUIDmig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.AlterColumn<string>(
                name: "RiddleUId",
                table: "Riddles",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Riddles_RiddleUId",
                table: "Riddles",
                column: "RiddleUId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Riddles_RiddleUId",
                table: "Riddles");

            migrationBuilder.AlterColumn<string>(
                name: "RiddleUId",
                table: "Riddles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Coin", "CreationDateTime", "ModificationDateTime", "Passwordhashed", "RemoveTime", "Role", "SerialNumber", "Username" },
                values: new object[] { 100, 0, new DateTime(2025, 10, 2, 17, 9, 26, 213, DateTimeKind.Local).AddTicks(5108), null, "fa585d89c851dd338a70dcf535aa2a92fee7836dd6aff1226583e88e0996293f16bc009c652826e0fc5c706695a03cddce372f139eff4d13959da6f1f5d3eabe", null, 1, "68e2afabfb", "Admin" });
        }
    }
}
