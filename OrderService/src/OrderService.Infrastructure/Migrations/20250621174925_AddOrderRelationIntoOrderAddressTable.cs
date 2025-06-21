using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderRelationIntoOrderAddressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId1",
                table: "OrderAddresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderAddresses_OrderId1",
                table: "OrderAddresses",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderAddresses_Orders_OrderId1",
                table: "OrderAddresses",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderAddresses_Orders_OrderId1",
                table: "OrderAddresses");

            migrationBuilder.DropIndex(
                name: "IX_OrderAddresses_OrderId1",
                table: "OrderAddresses");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderAddresses");
        }
    }
}
