using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentModel.Migrations
{
    /// <inheritdoc />
    public partial class Latestchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerOOGData",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    World = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerOOGData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCashRecords",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerOOGDataId = table.Column<uint>(type: "INTEGER", nullable: false),
                    StoredReal = table.Column<long>(type: "INTEGER", nullable: false),
                    StoredFake = table.Column<long>(type: "INTEGER", nullable: false),
                    InUseReal = table.Column<long>(type: "INTEGER", nullable: false),
                    InUseFake = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCashRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerCashRecords_PlayerOOGData_PlayerOOGDataId",
                        column: x => x.PlayerOOGDataId,
                        principalTable: "PlayerOOGData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerIdentifiers",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    World = table.Column<string>(type: "TEXT", nullable: false),
                    DateIdentityChanged = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlayerOOGDataId = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerIdentifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerIdentifiers_PlayerOOGData_PlayerOOGDataId",
                        column: x => x.PlayerOOGDataId,
                        principalTable: "PlayerOOGData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GilTransactions",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HostPlayerId = table.Column<uint>(type: "INTEGER", nullable: false),
                    PatronPlayerId = table.Column<uint>(type: "INTEGER", nullable: false),
                    IsRealGil = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cause = table.Column<int>(type: "INTEGER", nullable: false),
                    InUseDiff = table.Column<long>(type: "INTEGER", nullable: false),
                    StoredDiff = table.Column<long>(type: "INTEGER", nullable: false),
                    WhenUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PlayerCashRecordEntityId = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GilTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GilTransactions_PlayerCashRecords_PlayerCashRecordEntityId",
                        column: x => x.PlayerCashRecordEntityId,
                        principalTable: "PlayerCashRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GilTransactions_PlayerOOGData_HostPlayerId",
                        column: x => x.HostPlayerId,
                        principalTable: "PlayerOOGData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GilTransactions_PlayerOOGData_PatronPlayerId",
                        column: x => x.PatronPlayerId,
                        principalTable: "PlayerOOGData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_GilTransactions_PlayerCashRecordEntityId",
                table: "GilTransactions",
                column: "PlayerCashRecordEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCashRecords_PlayerOOGDataId",
                table: "PlayerCashRecords",
                column: "PlayerOOGDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerIdentifiers_Name",
                table: "PlayerIdentifiers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerIdentifiers_PlayerOOGDataId",
                table: "PlayerIdentifiers",
                column: "PlayerOOGDataId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerOOGData_Name",
                table: "PlayerOOGData",
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
                name: "PlayerCashRecords");

            migrationBuilder.DropTable(
                name: "PlayerOOGData");
        }
    }
}
