using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class RiddleNoMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "No",
                table: "Riddles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "RiddleUId",
                table: "Riddles",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                computedColumnSql: "[Department] + CAST([No] AS NVARCHAR(10))",
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No",
                table: "Riddles");

            migrationBuilder.AlterColumn<string>(
                name: "RiddleUId",
                table: "Riddles",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80,
                oldComputedColumnSql: "[Department] + CAST([No] AS NVARCHAR(10))");
        }
    }
}
