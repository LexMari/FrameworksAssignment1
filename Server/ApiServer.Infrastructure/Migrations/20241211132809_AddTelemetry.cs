using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTelemetry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelemetrySessions",
                columns: table => new
                {
                    TelemetrySessionId = table.Column<Guid>(type: "TEXT", maxLength: 25, nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    StartTimestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTimestamp = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlashcardSetId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetrySessions", x => x.TelemetrySessionId);
                    table.ForeignKey(
                        name: "FK_TelemetrySessions_FlashcardSets_FlashcardSetId",
                        column: x => x.FlashcardSetId,
                        principalTable: "FlashcardSets",
                        principalColumn: "FlashcardSetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TelemetrySessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ApiSettings",
                keyColumn: "ApiSettingId",
                keyValue: "SET_LIMIT_DAY",
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 12, 11, 13, 28, 9, 391, DateTimeKind.Local).AddTicks(153), new DateTime(2024, 12, 11, 13, 28, 9, 391, DateTimeKind.Local).AddTicks(203) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "4VxjQL/W7Bey8rK2k5LDi02agJoVXC5aqvwTX9E99e5jTTNtsvxluoA00fNy2LhzRcZU1cIWjcAjVgMnuZCTeg==", "+ckW8ClKRPJsoUbzSqXokO/JkinD7syQkEjxPU0HDyTNvDXmPUXs4GEMnVWB/OaShFopcpzMcktXJbmZmcJ9nw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "phbsGKaE7C+9dQPBqEIYNr0eefd+xtzGh2tfdPkwSmDqwygcJ0Y1YStexKyW1wG+sA+jeFNZz9kT0kssNA+BSw==", "7cvWOoVSpB8U6FJjqvZVtwYit55oI9mJdYzUhkPjg3a8XGqGM/RgOFYnZX+135liqec9ZfplPa+2t2/SnYs4YQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_TelemetrySessions_FlashcardSetId",
                table: "TelemetrySessions",
                column: "FlashcardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetrySessions_UserId",
                table: "TelemetrySessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelemetrySessions");

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
    }
}
