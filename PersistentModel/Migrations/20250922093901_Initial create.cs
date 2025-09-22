using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentModel.Migrations
{
    /// <inheritdoc />
    public partial class Initialcreate : Migration
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
                    PlayerOOGDataID = table.Column<uint>(type: "INTEGER", nullable: false),
                    StoredReal = table.Column<long>(type: "INTEGER", nullable: false),
                    StoredFake = table.Column<long>(type: "INTEGER", nullable: false),
                    InUseReal = table.Column<long>(type: "INTEGER", nullable: false),
                    InUseFake = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCashRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerCashRecords_PlayerOOGData_PlayerOOGDataID",
                        column: x => x.PlayerOOGDataID,
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
                    DateMetUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
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
                    HostPlayerId1 = table.Column<uint>(type: "INTEGER", nullable: false),
                    PlayerCashRecordId = table.Column<uint>(type: "INTEGER", nullable: false),
                    IsRealGil = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsHouseCut = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GilTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GilTransactions_PlayerCashRecords_PlayerCashRecordId",
                        column: x => x.PlayerCashRecordId,
                        principalTable: "PlayerCashRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GilTransactions_PlayerOOGData_HostPlayerId1",
                        column: x => x.HostPlayerId1,
                        principalTable: "PlayerOOGData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GilTransactions_HostPlayerId1",
                table: "GilTransactions",
                column: "HostPlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_GilTransactions_PlayerCashRecordId",
                table: "GilTransactions",
                column: "PlayerCashRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCashRecords_PlayerOOGDataID",
                table: "PlayerCashRecords",
                column: "PlayerOOGDataID",
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
