using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_.NET_nauka.Migrations
{
    /// <inheritdoc />
    public partial class Currencyrepair1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Change",
                table: "Currencies",
                type: "decimal(4,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Change",
                table: "Currencies",
                type: "decimal(4,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)");
        }
    }
}
