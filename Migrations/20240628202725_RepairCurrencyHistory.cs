using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_.NET_nauka.Migrations
{
    /// <inheritdoc />
    public partial class RepairCurrencyHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    Low = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Sum = table.Column<decimal>(type: "decimal(38,10)", nullable: false),
                    Measurement = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrenciesHistory",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrencyId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Low = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    High = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    AvgValue = table.Column<decimal>(type: "decimal(20,10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrenciesHistory", x => new { x.Date, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_CurrenciesHistory_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesHistory_CurrencyId",
                table: "CurrenciesHistory",
                column: "CurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrenciesHistory");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
