using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeveloperStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Sales_and_SalesItem_RemoveMoney_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "money");

            migrationBuilder.DropColumn(
                name: "TotalPrice_Currency",
                table: "saleitems");

            migrationBuilder.DropColumn(
                name: "UnitPrice_Currency",
                table: "saleitems");

            migrationBuilder.RenameColumn(
                name: "UnitPrice_Value",
                table: "saleitems",
                newName: "unitprice");

            migrationBuilder.RenameColumn(
                name: "TotalPrice_Value",
                table: "saleitems",
                newName: "totalprice");

            migrationBuilder.AddColumn<decimal>(
                name: "totalamount",
                table: "sales",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalamount",
                table: "sales");

            migrationBuilder.RenameColumn(
                name: "unitprice",
                table: "saleitems",
                newName: "UnitPrice_Value");

            migrationBuilder.RenameColumn(
                name: "totalprice",
                table: "saleitems",
                newName: "TotalPrice_Value");

            migrationBuilder.AddColumn<string>(
                name: "TotalPrice_Currency",
                table: "saleitems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UnitPrice_Currency",
                table: "saleitems",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "money",
                columns: table => new
                {
                    SaleId = table.Column<long>(type: "bigint", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_money", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_money_sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "sales",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
