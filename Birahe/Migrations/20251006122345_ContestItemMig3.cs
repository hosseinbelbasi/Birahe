using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class ContestItemMig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContestItem_Riddles_RiddleId",
                table: "ContestItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ContestItem_Users_UserId",
                table: "ContestItem");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 6, 15, 53, 44, 791, DateTimeKind.Local).AddTicks(9080), "d65e1ca487" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContestItem_Riddles_RiddleId",
                table: "ContestItem",
                column: "RiddleId",
                principalTable: "Riddles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContestItem_Users_UserId",
                table: "ContestItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContestItem_Riddles_RiddleId",
                table: "ContestItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ContestItem_Users_UserId",
                table: "ContestItem");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 6, 15, 38, 52, 118, DateTimeKind.Local).AddTicks(9912), "9b640de48a" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContestItem_Riddles_RiddleId",
                table: "ContestItem",
                column: "RiddleId",
                principalTable: "Riddles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContestItem_Users_UserId",
                table: "ContestItem",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
