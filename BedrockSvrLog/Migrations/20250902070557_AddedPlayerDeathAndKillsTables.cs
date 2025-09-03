using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedrockSvrLog.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlayerDeathAndKillsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerDeaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Xuid = table.Column<string>(type: "TEXT", nullable: false),
                    DeathTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Cause = table.Column<string>(type: "TEXT", nullable: false),
                    KillerXuid = table.Column<string>(type: "TEXT", nullable: true),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: true),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: true),
                    PositionZ = table.Column<int>(type: "INTEGER", nullable: true),
                    Demension = table.Column<string>(type: "TEXT", nullable: true),
                    GameTime = table.Column<int>(type: "INTEGER", nullable: false),
                    GameDay = table.Column<int>(type: "INTEGER", nullable: false),
                    SpawnPositionX = table.Column<int>(type: "INTEGER", nullable: true),
                    SpawnPositionY = table.Column<int>(type: "INTEGER", nullable: true),
                    SpawnPositionZ = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDeaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerDeaths_User_KillerXuid",
                        column: x => x.KillerXuid,
                        principalTable: "User",
                        principalColumn: "Xuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerDeaths_User_Xuid",
                        column: x => x.Xuid,
                        principalTable: "User",
                        principalColumn: "Xuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerKills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Xuid = table.Column<string>(type: "TEXT", nullable: false),
                    KillTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Cause = table.Column<string>(type: "TEXT", nullable: false),
                    EntityType = table.Column<string>(type: "TEXT", nullable: false),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: true),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: true),
                    PositionZ = table.Column<int>(type: "INTEGER", nullable: true),
                    Demension = table.Column<string>(type: "TEXT", nullable: true),
                    GameTime = table.Column<int>(type: "INTEGER", nullable: false),
                    GameDay = table.Column<int>(type: "INTEGER", nullable: false),
                    SpawnPositionX = table.Column<int>(type: "INTEGER", nullable: true),
                    SpawnPositionY = table.Column<int>(type: "INTEGER", nullable: true),
                    SpawnPositionZ = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerKills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerKills_User_Xuid",
                        column: x => x.Xuid,
                        principalTable: "User",
                        principalColumn: "Xuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDeaths_KillerXuid",
                table: "PlayerDeaths",
                column: "KillerXuid");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDeaths_Xuid",
                table: "PlayerDeaths",
                column: "Xuid");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerKills_Xuid",
                table: "PlayerKills",
                column: "Xuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerDeaths");

            migrationBuilder.DropTable(
                name: "PlayerKills");
        }
    }
}
