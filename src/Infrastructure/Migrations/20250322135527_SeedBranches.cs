using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeveloperStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedBranches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "branches",
                columns: new[] { "id", "address", "city", "name", "phone", "state", "zipcode" },
                values: new object[,]
                {
                    { 1, "123 Main St", "New York", "Main Branch", "123-456-7890", "NY", "10001" },
                    { 2, "456 Elm St", "Los Angeles", "Secondary Branch", "987-654-3210", "CA", "90001" }
                });

            //FIX: Update Branch id to max value
            migrationBuilder.Sql(@"SELECT setval('branches_id_seq', (SELECT coalesce(MAX(id),1) FROM branches));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "branches",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "branches",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
