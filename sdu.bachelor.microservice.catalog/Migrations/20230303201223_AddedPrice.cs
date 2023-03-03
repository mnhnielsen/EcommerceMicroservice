using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sdu.bachelor.microservice.catalog.Migrations
{
    /// <inheritdoc />
    public partial class AddedPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Products",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("7201fd50-25b9-4b7d-99a7-b367b73222f8"),
                column: "Price",
                value: 10000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("ab0f5a1f-9b48-4862-8e6a-bced8d20558e"),
                column: "Price",
                value: 11000.0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d4b1d999-862d-4cf9-bcb7-b79de08768b9"),
                column: "Price",
                value: 12000.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");
        }
    }
}
