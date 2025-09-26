using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class UserSerialNumberMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Users");
        }
    }
}
