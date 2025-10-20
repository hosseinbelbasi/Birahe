using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class ContestItemCorrection : Migration
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContestItem",
                table: "ContestItem");

            migrationBuilder.DropIndex(
                name: "IX_ContestItem_UserId",
                table: "ContestItem");

            migrationBuilder.RenameTable(
                name: "ContestItem",
                newName: "ContestItems");

            migrationBuilder.RenameIndex(
                name: "IX_ContestItem_RiddleId",
                table: "ContestItems",
                newName: "IX_ContestItems_RiddleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContestItems",
                table: "ContestItems",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 7, 20, 49, 2, 53, DateTimeKind.Local).AddTicks(6726), "fd90214896" });

            migrationBuilder.CreateIndex(
                name: "IX_ContestItems_UserId_RiddleId",
                table: "ContestItems",
                columns: new[] { "UserId", "RiddleId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContestItems_Riddles_RiddleId",
                table: "ContestItems",
                column: "RiddleId",
                principalTable: "Riddles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContestItems_Users_UserId",
                table: "ContestItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContestItems_Riddles_RiddleId",
                table: "ContestItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ContestItems_Users_UserId",
                table: "ContestItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContestItems",
                table: "ContestItems");

            migrationBuilder.DropIndex(
                name: "IX_ContestItems_UserId_RiddleId",
                table: "ContestItems");

            migrationBuilder.RenameTable(
                name: "ContestItems",
                newName: "ContestItem");

            migrationBuilder.RenameIndex(
                name: "IX_ContestItems_RiddleId",
                table: "ContestItem",
                newName: "IX_ContestItem_RiddleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContestItem",
                table: "ContestItem",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 6, 15, 53, 44, 791, DateTimeKind.Local).AddTicks(9080), "d65e1ca487" });

            migrationBuilder.CreateIndex(
                name: "IX_ContestItem_UserId",
                table: "ContestItem",
                column: "UserId");

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
    }
}
