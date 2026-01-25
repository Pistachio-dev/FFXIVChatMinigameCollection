using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistentModel.Migrations
{
    /// <inheritdoc />
    public partial class TransactionRefactor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsHouseCut",
                table: "GilTransactions",
                newName: "StoredDiff");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "GilTransactions",
                newName: "InUseDiff");

            migrationBuilder.AddColumn<int>(
                name: "Cause",
                table: "GilTransactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cause",
                table: "GilTransactions");

            migrationBuilder.RenameColumn(
                name: "StoredDiff",
                table: "GilTransactions",
                newName: "IsHouseCut");

            migrationBuilder.RenameColumn(
                name: "InUseDiff",
                table: "GilTransactions",
                newName: "Amount");
        }
    }
}
