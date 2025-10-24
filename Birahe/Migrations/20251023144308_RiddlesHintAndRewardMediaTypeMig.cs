using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birahe.EndPoint.Migrations
{
    /// <inheritdoc />
    public partial class RiddlesHintAndRewardMediaTypeMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RewardImageFileName",
                table: "Riddles",
                newName: "RewardFileName");

            migrationBuilder.RenameColumn(
                name: "HintImageFileName",
                table: "Riddles",
                newName: "HintFileName");

            migrationBuilder.AddColumn<int>(
                name: "HintMediaType",
                table: "Riddles",
                type: "int",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RewardMediaType",
                table: "Riddles",
                type: "int",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HintMediaType",
                table: "Riddles");

            migrationBuilder.DropColumn(
                name: "RewardMediaType",
                table: "Riddles");

            migrationBuilder.RenameColumn(
                name: "RewardFileName",
                table: "Riddles",
                newName: "RewardImageFileName");

            migrationBuilder.RenameColumn(
                name: "HintFileName",
                table: "Riddles",
                newName: "HintImageFileName");
        }
    }
}
