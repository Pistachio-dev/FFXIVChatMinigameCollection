using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentModel.Migrations
{
    /// <inheritdoc />
    public partial class Changedrelationshipfromcashrecordtoplayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GilTransactions_PlayerCashRecords_PlayerCashRecordId",
                table: "GilTransactions");

            migrationBuilder.RenameColumn(
                name: "PlayerCashRecordId",
                table: "GilTransactions",
                newName: "PatronPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_GilTransactions_PlayerCashRecordId",
                table: "GilTransactions",
                newName: "IX_GilTransactions_PatronPlayerId");

            migrationBuilder.AddColumn<uint>(
                name: "PlayerCashRecordEntityId",
                table: "GilTransactions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GilTransactions_PlayerCashRecordEntityId",
                table: "GilTransactions",
                column: "PlayerCashRecordEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_GilTransactions_PlayerCashRecords_PlayerCashRecordEntityId",
                table: "GilTransactions",
                column: "PlayerCashRecordEntityId",
                principalTable: "PlayerCashRecords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GilTransactions_PlayerOOGData_PatronPlayerId",
                table: "GilTransactions",
                column: "PatronPlayerId",
                principalTable: "PlayerOOGData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GilTransactions_PlayerCashRecords_PlayerCashRecordEntityId",
                table: "GilTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_GilTransactions_PlayerOOGData_PatronPlayerId",
                table: "GilTransactions");

            migrationBuilder.DropIndex(
                name: "IX_GilTransactions_PlayerCashRecordEntityId",
                table: "GilTransactions");

            migrationBuilder.DropColumn(
                name: "PlayerCashRecordEntityId",
                table: "GilTransactions");

            migrationBuilder.RenameColumn(
                name: "PatronPlayerId",
                table: "GilTransactions",
                newName: "PlayerCashRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_GilTransactions_PatronPlayerId",
                table: "GilTransactions",
                newName: "IX_GilTransactions_PlayerCashRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_GilTransactions_PlayerCashRecords_PlayerCashRecordId",
                table: "GilTransactions",
                column: "PlayerCashRecordId",
                principalTable: "PlayerCashRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
