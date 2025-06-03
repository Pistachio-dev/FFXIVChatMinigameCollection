using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentModel.Migrations
{
    /// <inheritdoc />
    public partial class Initialstate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerCashRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    RealGilBalance = table.Column<long>(type: "INTEGER", nullable: false),
                    FakeGilBalance = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCashRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerOOGEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    World = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CashRecordId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerOOGEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerOOGEntries_PlayerCashRecords_CashRecordId",
                        column: x => x.CashRecordId,
                        principalTable: "PlayerCashRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GilTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    HostPlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PatronPlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsRealGil = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsHouseCut = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlayerCashRecordId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GilTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GilTransactions_PlayerCashRecords_PlayerCashRecordId",
                        column: x => x.PlayerCashRecordId,
                        principalTable: "PlayerCashRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GilTransactions_PlayerOOGEntries_HostPlayerId",
                        column: x => x.HostPlayerId,
                        principalTable: "PlayerOOGEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GilTransactions_PlayerOOGEntries_PatronPlayerId",
                        column: x => x.PatronPlayerId,
                        principalTable: "PlayerOOGEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerIdentifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    World = table.Column<string>(type: "TEXT", nullable: false),
                    DateMetUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlayerOOGDataId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerIdentifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerIdentifiers_PlayerOOGEntries_PlayerOOGDataId",
                        column: x => x.PlayerOOGDataId,
                        principalTable: "PlayerOOGEntries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GilTransactions_HostPlayerId",
                table: "GilTransactions",
                column: "HostPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GilTransactions_PatronPlayerId",
                table: "GilTransactions",
                column: "PatronPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GilTransactions_PlayerCashRecordId",
                table: "GilTransactions",
                column: "PlayerCashRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerIdentifiers_Name",
                table: "PlayerIdentifiers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerIdentifiers_PlayerOOGDataId",
                table: "PlayerIdentifiers",
                column: "PlayerOOGDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerOOGEntries_CashRecordId",
                table: "PlayerOOGEntries",
                column: "CashRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerOOGEntries_Name",
                table: "PlayerOOGEntries",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GilTransactions");

            migrationBuilder.DropTable(
                name: "PlayerIdentifiers");

            migrationBuilder.DropTable(
                name: "PlayerOOGEntries");

            migrationBuilder.DropTable(
                name: "PlayerCashRecords");
        }
    }
}
