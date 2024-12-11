using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "FlashcardSets",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Comments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ApiSettings",
                keyColumn: "ApiSettingId",
                keyValue: "SET_LIMIT_DAY",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 11, 10, 38, 56, 913, DateTimeKind.Local).AddTicks(1199), new DateTime(2024, 12, 11, 10, 38, 56, 913, DateTimeKind.Local).AddTicks(1244) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "keEylKKFJGqonuUMVr9DfsefesIBRBcb+o80PhTQGnq/D0hecj+eUU5/5ATVJPfjvPXW6+fNNB0r/w0Gh1mWSQ==", "rozJk1nfLVMg3Ppf2UTcF2WBbwcUDRize72Ap0WawiXsnIjRmODrzX+g4BYdpYuB1dLPIvlA2p8Y0lpuSBJXug==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "yWWMDneBej5l9rIt3bIPSTOXZqKyp8bofe4o+yqn+ztp+5gyusoBTjc38OHcAHfej7MeMOqvWqWHp4PvRCCrGw==", "kY6VoWsSBJERN2Bt4ej633PpO0Mpq/Hze54WUNSaqYOxoBw4V8LWGG64Rrx6ahDzKGcFtxbEt4b1vlBEoq0ONA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "FlashcardSets");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "ApiSettings",
                keyColumn: "ApiSettingId",
                keyValue: "SET_LIMIT_DAY",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 27, 14, 59, 17, 349, DateTimeKind.Local).AddTicks(9302), new DateTime(2024, 11, 27, 14, 59, 17, 349, DateTimeKind.Local).AddTicks(9351) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "1mvcs1kEiT1X/+4KvAEpI0Yea0JCa7x+2hpZGihm/pK7qrBWbtzBJwN7NSRcrecKKDN/5kuSQzPQZ76CjT95cg==", "AHdtwHiJ78BgfgyL87+6nn+BXrCxa4H8vgcARPFooovHfHed1gYy+tiyG3h1+AB7K5434O707q9vPP02UgVZ2g==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "3jtSiy1yZMxVe0b9LhexIV5FHL+/im0Py7DCTfGAqQSMRN91aLqQ9Xa+DLagHImiTwu2+aaWEwz80SAoPlclIQ==", "hG+4wXFAbdNVYgaffYJPRqzK70s4mhWJAn3nn9Bfn7TEp4VKxItVYlWXPgeIDNBLxTTyKZUimXMjbj4LNrFKNg==" });
        }
    }
}
