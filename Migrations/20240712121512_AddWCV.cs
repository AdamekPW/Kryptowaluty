using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_.NET_nauka.Migrations
{
    /// <inheritdoc />
    public partial class AddWCV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletCurrencyValue_Currencies_CurrencyId",
                table: "WalletCurrencyValue");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletCurrencyValue_Wallets_UserId",
                table: "WalletCurrencyValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletCurrencyValue",
                table: "WalletCurrencyValue");

            migrationBuilder.RenameTable(
                name: "WalletCurrencyValue",
                newName: "WalletCurrencyValues");

            migrationBuilder.RenameIndex(
                name: "IX_WalletCurrencyValue_CurrencyId",
                table: "WalletCurrencyValues",
                newName: "IX_WalletCurrencyValues_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletCurrencyValues",
                table: "WalletCurrencyValues",
                columns: new[] { "UserId", "CurrencyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WalletCurrencyValues_Currencies_CurrencyId",
                table: "WalletCurrencyValues",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletCurrencyValues_Wallets_UserId",
                table: "WalletCurrencyValues",
                column: "UserId",
                principalTable: "Wallets",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletCurrencyValues_Currencies_CurrencyId",
                table: "WalletCurrencyValues");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletCurrencyValues_Wallets_UserId",
                table: "WalletCurrencyValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletCurrencyValues",
                table: "WalletCurrencyValues");

            migrationBuilder.RenameTable(
                name: "WalletCurrencyValues",
                newName: "WalletCurrencyValue");

            migrationBuilder.RenameIndex(
                name: "IX_WalletCurrencyValues_CurrencyId",
                table: "WalletCurrencyValue",
                newName: "IX_WalletCurrencyValue_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletCurrencyValue",
                table: "WalletCurrencyValue",
                columns: new[] { "UserId", "CurrencyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WalletCurrencyValue_Currencies_CurrencyId",
                table: "WalletCurrencyValue",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalletCurrencyValue_Wallets_UserId",
                table: "WalletCurrencyValue",
                column: "UserId",
                principalTable: "Wallets",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
