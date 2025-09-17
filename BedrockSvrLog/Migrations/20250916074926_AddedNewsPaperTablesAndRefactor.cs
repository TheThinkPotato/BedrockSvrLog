using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedrockSvrLog.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewsPaperTablesAndRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Paper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PublishDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paper", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerXuid = table.Column<string>(type: "TEXT", nullable: true),
                    PlayerDeathId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerKillsId = table.Column<int>(type: "INTEGER", nullable: true),
                    RealmEventId = table.Column<int>(type: "INTEGER", nullable: true),
                    PaperId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Subtitle = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    GameDay = table.Column<int>(type: "INTEGER", nullable: false),
                    GameTime = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerSpawnX = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerSpawnY = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerSpawnZ = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Article_Paper_Id",
                        column: x => x.Id,
                        principalTable: "Paper",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Article_PlayerDeaths_PlayerDeathId",
                        column: x => x.PlayerDeathId,
                        principalTable: "PlayerDeaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Article_PlayerKills_PlayerKillsId",
                        column: x => x.PlayerKillsId,
                        principalTable: "PlayerKills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Article_RealmEvent_RealmEventId",
                        column: x => x.RealmEventId,
                        principalTable: "RealmEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Article_User_PlayerXuid",
                        column: x => x.PlayerXuid,
                        principalTable: "User",
                        principalColumn: "Xuid",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Article_PlayerDeathId",
                table: "Article",
                column: "PlayerDeathId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_PlayerKillsId",
                table: "Article",
                column: "PlayerKillsId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_PlayerXuid",
                table: "Article",
                column: "PlayerXuid");

            migrationBuilder.CreateIndex(
                name: "IX_Article_RealmEventId",
                table: "Article",
                column: "RealmEventId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Paper");

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
    }
}
