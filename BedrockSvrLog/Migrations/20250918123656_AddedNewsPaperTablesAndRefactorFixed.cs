using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BedrockSvrLog.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewsPaperTablesAndRefactorFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Paper_Id",
                table: "Article");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Article",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.CreateIndex(
                name: "IX_Article_PaperId",
                table: "Article",
                column: "PaperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Paper_PaperId",
                table: "Article",
                column: "PaperId",
                principalTable: "Paper",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Paper_PaperId",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_PaperId",
                table: "Article");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Article",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Paper_Id",
                table: "Article",
                column: "Id",
                principalTable: "Paper",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
