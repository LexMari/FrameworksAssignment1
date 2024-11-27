using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Username",
                table: "Users");

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

            migrationBuilder.CreateIndex(
                name: "UX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Users_Username",
                table: "Users");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Username",
                table: "Users",
                column: "Username");

            migrationBuilder.UpdateData(
                table: "ApiSettings",
                keyColumn: "ApiSettingId",
                keyValue: "SET_LIMIT_DAY",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 22, 20, 12, 24, 249, DateTimeKind.Local).AddTicks(7191), new DateTime(2024, 11, 22, 20, 12, 24, 249, DateTimeKind.Local).AddTicks(7237) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "QNhzDGAVqPFisUaV4H+HY6IHfna/ug6gsgWsdGLyYZjEpH7OZ1XgJkZpKhxC1WNEOGn8xyoM6Dfs6oWpibqdtQ==", "I2jB8xAYjrde9ZtKPwWClm9fmap2aZFGxGuxxSPkSbeAOiTiKGjiRZ8zeIVc8lfVa23ePAy0FTxGc8XmbQ2PQg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "NkcCWrC/g7vovBrsz6J6bvjeEZYKNNopeFRJcZQwTdyHi+OdsrtcMzegzBsjKyekML0h8MNb/Uo3LX+j5W7gxQ==", "ACADmf2Sg6Gi1IyW+/lYoQPE3MU6/zaoSyErSDNE2YD8UR7voVYxgugDtZRCwaXLFkhLZ3xSIviLCvYTEvKf1g==" });
        }
    }
}
