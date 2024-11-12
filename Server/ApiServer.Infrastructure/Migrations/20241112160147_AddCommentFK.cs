using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_FlashcardSets_FlashcardSetId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "FlashcardSetId",
                table: "Comments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "wS/VFfxLIwCQCpcM14ZqY5GTLfuAupvRb03p7/GuG+q24/UnnQ8iw8VzY1rd5+hxxQ9RnKUsWnJdaWpDnskRow==", "oE0HB93xxLE26qDKKJhmrqZxD2cetuWm3CCcJLgAjYNTelRe567ZmWrw3XjAsEsJL4poYq+VtN7T1YHim7hvnw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "+Jcb1KtdIm3IuADxV689DBBcrRx8e8g8jzLuytMPgdzgmKYiAdm3djiRLLXrST0smLa7df6U1McATGk/Dx6MpA==", "3bYWppb8y/hQ+PV041c2Pn0uxcAD7FtH/b+w/sQvZ8RWfrl8wPZ/uQXVbT3PFLzd61AznTYRS981A1muyWux7w==" });

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

            migrationBuilder.AlterColumn<int>(
                name: "FlashcardSetId",
                table: "Comments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "w3zLSWUZ4dLP20icJT593RgHRq7buV0nXKRbf10uxpf8flMhTuegV94NUdUSHHDW0FbinvVAINAaX0B9mjo0dQ==", "cYeT914bXdEvj7TYm4UWuOsICmhUrcsQqP6UXjsy4GKXRVJeG9u93OT5mpQ9UBBcoYl3g7h4icOi50zjTHL+MA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "TXNqvMXo1b5mH7O7/iI8UquPY3XR/2sNNtvk5y/F5SFskW9CaFM0wasY+tP132RGbycLBxIeh1pPLDSzspadgg==", "sapB/Ozr4IJsQflqc/+yfekih1w2eMIqqrvNJ9smmExxRZMB7oGuogMUVkfu7SdB/gk97meNI4WaHCEMzfjyMg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_FlashcardSets_FlashcardSetId",
                table: "Comments",
                column: "FlashcardSetId",
                principalTable: "FlashcardSets",
                principalColumn: "FlashcardSetId");
        }
    }
}
