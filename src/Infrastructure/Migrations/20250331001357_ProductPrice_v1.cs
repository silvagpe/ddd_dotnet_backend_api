using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeveloperStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProductPrice_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_money_products_ProductId",
                table: "money");

            migrationBuilder.DropColumn(
                name: "TotalAmount_Currency",
                table: "sales");

            migrationBuilder.DropColumn(
                name: "TotalAmount_Value",
                table: "sales");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "money",
                newName: "SaleId");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "products",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_money_sales_SaleId",
            //     table: "money",
            //     column: "SaleId",
            //     principalTable: "sales",
            //     principalColumn: "id",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_money_sales_SaleId",
                table: "money");

            migrationBuilder.DropColumn(
                name: "price",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "SaleId",
                table: "money",
                newName: "ProductId");

            migrationBuilder.AddColumn<string>(
                name: "TotalAmount_Currency",
                table: "sales",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount_Value",
                table: "sales",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_money_products_ProductId",
                table: "money",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
