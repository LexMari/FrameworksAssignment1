using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CollectionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.CollectionId);
                    table.ForeignKey(
                        name: "FK_Collections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionFlashcard",
                columns: table => new
                {
                    CollectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlashcardSetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionFlashcard", x => new { x.CollectionId, x.FlashcardSetId });
                    table.ForeignKey(
                        name: "FK_CollectionFlashcard_Collections_CollectionId",
                        column: x => x.CollectionId,
                        principalTable: "Collections",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CollectionFlashcard_FlashcardSets_FlashcardSetId",
                        column: x => x.FlashcardSetId,
                        principalTable: "FlashcardSets",
                        principalColumn: "FlashcardSetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "wwK6NYLzwN5rb+juICmtobHHMxVIq9IUQWleoHnvFiQn0vOOr7zg5arinQus9yTtBfOPkaKC0Mw0TEPmA/zG0w==", "SYkU/hsAl/aVSN/x28iKzZL9KEfTBFX88HAmH93/ts8CSBuASHudbOGqcE8dP09NCnvAIOwIpVanJA8cjyobog==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "4sjT6rkLJz9Ju8kKVyCs6Xki7N6CQt4U4T3/0Ug7DjQ44bivrd5NNDasBpCs2wc3g51aGfDZsfZyutpkCvUABw==", "se0qMF8l9306yusxLZHekY9Qgq1ju4P3YYULaK4Vq3YpdRxO+aYsSMNoFkIfw0PilsFhJvXlcrHspniiyhnbEw==" });

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSets_UserId",
                table: "FlashcardSets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionFlashcard_FlashcardSetId",
                table: "CollectionFlashcard",
                column: "FlashcardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Collections_UserId",
                table: "Collections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardSets_Users_UserId",
                table: "FlashcardSets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardSets_Users_UserId",
                table: "FlashcardSets");

            migrationBuilder.DropTable(
                name: "CollectionFlashcard");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropIndex(
                name: "IX_FlashcardSets_UserId",
                table: "FlashcardSets");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "Bcl9pzjWyF+hwwoPNBsMpzMjDpyoTumbu9gax4jKdzmxb8h40sQSfx+bB8Jtlfmibvo4ESu6qQ3yaqa/cNqWqg==", "sURAlFJBbRuUaFqP7aWb7/LJXWyhzgWmTRDIqDRfATbnYuZLD10sPZNwwwO4b3K86tKhrpQKQvWH5jWWa66yXA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "AwesMGaaR+bzs86tTsmfCZAVv/qPp25+qCKyUh7a+v9bU50lI35uWj4HjihsnyxyZNI3MEJATMNwcextCIkKow==", "zGvBg/64Dgu6RtigwdIz5+UBPG8EX8Nc9prWxMcEPhKADskNAcjBf75xo2XhbnfipIJEHj+emM+YLodKQoGkgQ==" });
        }
    }
}
