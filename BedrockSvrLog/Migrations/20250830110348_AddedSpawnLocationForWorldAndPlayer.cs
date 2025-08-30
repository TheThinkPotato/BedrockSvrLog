using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedrockSvrLog.Migrations
{
    /// <inheritdoc />
    public partial class AddedSpawnLocationForWorldAndPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpawnX",
                table: "World",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpawnY",
                table: "World",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpawnZ",
                table: "World",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpawnX",
                table: "User",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpawnY",
                table: "User",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpawnZ",
                table: "User",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpawnX",
                table: "World");

            migrationBuilder.DropColumn(
                name: "SpawnY",
                table: "World");

            migrationBuilder.DropColumn(
                name: "SpawnZ",
                table: "World");

            migrationBuilder.DropColumn(
                name: "SpawnX",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SpawnY",
                table: "User");

            migrationBuilder.DropColumn(
                name: "SpawnZ",
                table: "User");
        }
    }
}
