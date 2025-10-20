using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class ContestItemMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContestItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RiddleId = table.Column<int>(type: "int", nullable: false),
                    OpeningDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSolved = table.Column<bool>(type: "bit", nullable: false),
                    SolvingDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tries = table.Column<int>(type: "int", nullable: false),
                    LastTryDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HasOpenedHint = table.Column<bool>(type: "bit", nullable: false),
                    OpeningHintDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContestItem_Riddles_RiddleId",
                        column: x => x.RiddleId,
                        principalTable: "Riddles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContestItem_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 6, 15, 35, 29, 645, DateTimeKind.Local).AddTicks(8647), "6e359073a3" });

            migrationBuilder.CreateIndex(
                name: "IX_ContestItem_RiddleId",
                table: "ContestItem",
                column: "RiddleId");

            migrationBuilder.CreateIndex(
                name: "IX_ContestItem_UserId",
                table: "ContestItem",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContestItem");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 6, 0, 30, 38, 152, DateTimeKind.Local).AddTicks(2270), "6b91cc3f37" });
        }
    }
}
