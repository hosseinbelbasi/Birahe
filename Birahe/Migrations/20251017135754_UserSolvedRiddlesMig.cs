using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class UserSolvedRiddlesMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SolvedRiddles",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "CreationDateTime", "SerialNumber", "SolvedRiddles" },
                values: new object[] { new DateTime(2025, 10, 17, 17, 27, 54, 451, DateTimeKind.Local).AddTicks(4997), "0b1b8a65aa", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SolvedRiddles",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1000,
                columns: new[] { "CreationDateTime", "SerialNumber" },
                values: new object[] { new DateTime(2025, 10, 17, 15, 30, 53, 826, DateTimeKind.Local).AddTicks(1358), "cdaef4a39e" });
        }
    }
}
