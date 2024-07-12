using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_.NET_nauka.Migrations
{
    /// <inheritdoc />
    public partial class RepairValueInWCV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "WalletCurrencyValues",
                type: "decimal(20,10)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "WalletCurrencyValues");
        }
    }
}
