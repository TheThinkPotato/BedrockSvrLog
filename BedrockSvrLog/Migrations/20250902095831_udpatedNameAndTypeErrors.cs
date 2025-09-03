using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedrockSvrLog.Migrations
{
    /// <inheritdoc />
    public partial class udpatedNameAndTypeErrors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Demension",
                table: "PlayerKills",
                newName: "Dimension");

            migrationBuilder.RenameColumn(
                name: "Demension",
                table: "PlayerDeaths",
                newName: "Dimension");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentTime",
                table: "World",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "GameTime",
                table: "PlayerKills",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "GameTime",
                table: "PlayerDeaths",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dimension",
                table: "PlayerKills",
                newName: "Demension");

            migrationBuilder.RenameColumn(
                name: "Dimension",
                table: "PlayerDeaths",
                newName: "Demension");

            migrationBuilder.AlterColumn<string>(
                name: "CurrentTime",
                table: "World",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GameTime",
                table: "PlayerKills",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GameTime",
                table: "PlayerDeaths",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
