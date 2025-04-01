using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeveloperStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Customers_seed_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "id", "email", "firstname", "lastname", "phone" },
                values: new object[,]
                {
                    { 1L, "john.doe@example.com", "John", "Doe", "123-456-7890" },
                    { 2L, "jane.smith@example.com", "Jane", "Smith", "987-654-3210" },
                    { 3L, "alice.johnson@example.com", "Alice", "Johnson", "555-123-4567" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "id",
                keyValue: 3L);
        }
    }
}
