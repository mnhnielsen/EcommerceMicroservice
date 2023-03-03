using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sdu.bachelor.microservice.catalog.Migrations
{
    /// <inheritdoc />
    public partial class AddedStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("7201fd50-25b9-4b7d-99a7-b367b73222f8"),
                column: "Stock",
                value: 5);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("ab0f5a1f-9b48-4862-8e6a-bced8d20558e"),
                column: "Stock",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("d4b1d999-862d-4cf9-bcb7-b79de08768b9"),
                column: "Stock",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Products");
        }
    }
}
