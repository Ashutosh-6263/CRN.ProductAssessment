using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRN.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseConstraints : Migration
    {
        /// <inheritdoc CreateIndex(
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductName",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                column: "ProductName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductName",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                column: "ProductName");
        }
    }
}
