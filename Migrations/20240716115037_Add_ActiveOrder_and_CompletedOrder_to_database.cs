using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_.NET_nauka.Migrations
{
    /// <inheritdoc />
    public partial class Add_ActiveOrder_and_CompletedOrder_to_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "ActiveOrders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qty = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    QtyUSDT = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    DateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveOrders", x => x.id);
                    table.ForeignKey(
                        name: "FK_ActiveOrders_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActiveOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedOrders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Qty = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    QtyUSDT = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    DateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedOrders", x => x.id);
                    table.ForeignKey(
                        name: "FK_CompletedOrders_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedOrders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_ActiveOrders_CurrencyId",
                table: "ActiveOrders",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveOrders_UserId",
                table: "ActiveOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedOrders_CurrencyId",
                table: "CompletedOrders",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedOrders_UserId",
                table: "CompletedOrders",
                column: "UserId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveOrders");

            migrationBuilder.DropTable(
                name: "CompletedOrders");

            
        }
    }
}
