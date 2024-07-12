using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_.NET_nauka.Migrations
{
    /// <inheritdoc />
    public partial class Add_wallet_and_walletCurrencyValue_to_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalValue = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    latestUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalletCurrencyValue",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletCurrencyValue", x => new { x.UserId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_WalletCurrencyValue_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalletCurrencyValue_Wallets_UserId",
                        column: x => x.UserId,
                        principalTable: "Wallets",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletCurrencyValue_CurrencyId",
                table: "WalletCurrencyValue",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WalletCurrencyValue");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
