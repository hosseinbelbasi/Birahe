using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class RiddleAnswerFormatMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Format",
                table: "Riddles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Format",
                table: "Riddles");
        }
    }
}
