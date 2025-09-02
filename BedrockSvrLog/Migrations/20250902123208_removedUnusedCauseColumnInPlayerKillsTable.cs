using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedrockSvrLog.Migrations
{
    /// <inheritdoc />
    public partial class removedUnusedCauseColumnInPlayerKillsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cause",
                table: "PlayerKills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cause",
                table: "PlayerKills",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
