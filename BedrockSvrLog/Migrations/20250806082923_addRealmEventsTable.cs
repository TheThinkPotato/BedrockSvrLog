using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedrockSvrLog.Migrations
{
    /// <inheritdoc />
    public partial class addRealmEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RealmEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Xuid = table.Column<string>(type: "TEXT", nullable: false),
                    EventType = table.Column<string>(type: "TEXT", nullable: false),
                    EventTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealmEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealmEvent_User_Xuid",
                        column: x => x.Xuid,
                        principalTable: "User",
                        principalColumn: "Xuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealmEvent_Xuid",
                table: "RealmEvent",
                column: "Xuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealmEvent");
        }
    }
}
