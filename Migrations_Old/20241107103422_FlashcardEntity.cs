using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FlashcardEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashCard_FlashcardSets_FlashcardSetId",
                table: "FlashCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlashCard",
                table: "FlashCard");

            migrationBuilder.RenameTable(
                name: "FlashCard",
                newName: "Flashcards");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Flashcards",
                newName: "FlashcardId");

            migrationBuilder.RenameIndex(
                name: "IX_FlashCard_FlashcardSetId",
                table: "Flashcards",
                newName: "IX_Flashcards_FlashcardSetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flashcards",
                table: "Flashcards",
                column: "FlashcardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetId",
                table: "Flashcards",
                column: "FlashcardSetId",
                principalTable: "FlashcardSets",
                principalColumn: "FlashcardSetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardSets_FlashcardSetId",
                table: "Flashcards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flashcards",
                table: "Flashcards");

            migrationBuilder.RenameTable(
                name: "Flashcards",
                newName: "FlashCard");

            migrationBuilder.RenameColumn(
                name: "FlashcardId",
                table: "FlashCard",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_FlashcardSetId",
                table: "FlashCard",
                newName: "IX_FlashCard_FlashcardSetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlashCard",
                table: "FlashCard",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashCard_FlashcardSets_FlashcardSetId",
                table: "FlashCard",
                column: "FlashcardSetId",
                principalTable: "FlashcardSets",
                principalColumn: "FlashcardSetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
