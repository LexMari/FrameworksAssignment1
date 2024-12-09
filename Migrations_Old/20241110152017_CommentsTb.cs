using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CommentsTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_FlashcardSets_FlashcardSetId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_FlashcardSetId",
                table: "Comments",
                newName: "IX_Comments_FlashcardSetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_FlashcardSets_FlashcardSetId",
                table: "Comments",
                column: "FlashcardSetId",
                principalTable: "FlashcardSets",
                principalColumn: "FlashcardSetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_FlashcardSets_FlashcardSetId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comment",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_FlashcardSetId",
                table: "Comment",
                newName: "IX_Comment_FlashcardSetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_FlashcardSets_FlashcardSetId",
                table: "Comment",
                column: "FlashcardSetId",
                principalTable: "FlashcardSets",
                principalColumn: "FlashcardSetId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
