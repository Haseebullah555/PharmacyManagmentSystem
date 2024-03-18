using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class add_Medicine_expiryDate_add_sale_amount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Sales",
                newName: "SaleAmount");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpiryDate",
                table: "Purchases",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Purchases");

            migrationBuilder.RenameColumn(
                name: "SaleAmount",
                table: "Sales",
                newName: "Amount");
        }
    }
}
