using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedrockSvrLog.Migrations
{
    /// <inheritdoc />
    public partial class FixPlayerNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDeaths_User_KillerXuid1",
                table: "PlayerDeaths");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDeaths_User_PlayerXuid",
                table: "PlayerDeaths");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerKills_User_PlayerXuid",
                table: "PlayerKills");

            migrationBuilder.DropIndex(
                name: "IX_PlayerKills_PlayerXuid",
                table: "PlayerKills");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDeaths_KillerXuid1",
                table: "PlayerDeaths");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDeaths_PlayerXuid",
                table: "PlayerDeaths");

            migrationBuilder.DropColumn(
                name: "PlayerXuid",
                table: "PlayerKills");

            migrationBuilder.DropColumn(
                name: "KillerXuid1",
                table: "PlayerDeaths");

            migrationBuilder.DropColumn(
                name: "PlayerXuid",
                table: "PlayerDeaths");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlayerXuid",
                table: "PlayerKills",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KillerXuid1",
                table: "PlayerDeaths",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerXuid",
                table: "PlayerDeaths",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerKills_PlayerXuid",
                table: "PlayerKills",
                column: "PlayerXuid");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDeaths_KillerXuid1",
                table: "PlayerDeaths",
                column: "KillerXuid1");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDeaths_PlayerXuid",
                table: "PlayerDeaths",
                column: "PlayerXuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDeaths_User_KillerXuid1",
                table: "PlayerDeaths",
                column: "KillerXuid1",
                principalTable: "User",
                principalColumn: "Xuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDeaths_User_PlayerXuid",
                table: "PlayerDeaths",
                column: "PlayerXuid",
                principalTable: "User",
                principalColumn: "Xuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerKills_User_PlayerXuid",
                table: "PlayerKills",
                column: "PlayerXuid",
                principalTable: "User",
                principalColumn: "Xuid");
        }
    }
}
