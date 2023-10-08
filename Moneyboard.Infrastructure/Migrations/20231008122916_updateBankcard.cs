using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moneyboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateBankcard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CandNumber",
                table: "BankCard",
                newName: "CardNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CardNumber",
                table: "BankCard",
                newName: "CandNumber");
        }
    }
}
