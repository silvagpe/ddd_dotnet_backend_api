using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeveloperStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Product_drop_name_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
