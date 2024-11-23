using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApiSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiSettings",
                columns: table => new
                {
                    ApiSettingId = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiSettings", x => x.ApiSettingId);
                });

            migrationBuilder.InsertData(
                table: "ApiSettings",
                columns: new[] { "ApiSettingId", "CreatedAt", "Description", "Type", "UpdatedAt", "Value" },
                values: new object[] { "SET_LIMIT_DAY", new DateTime(2024, 11, 22, 20, 12, 24, 249, DateTimeKind.Local).AddTicks(7191), "The maximum number of sets that can be created per day by a user. Zero for unlimited", "Integer", new DateTime(2024, 11, 22, 20, 12, 24, 249, DateTimeKind.Local).AddTicks(7237), "20" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiSettings");

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
        }
    }
}
