using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace sdu.bachelor.microservice.catalog.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2500, nullable: true),
                    BrandId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("cd3eb3f1-0143-495b-9b90-9d1e8e46fbad"), "Trek" },
                    { new Guid("e29de237-8203-4e3e-8066-4ac71d2c707f"), "Factor" },
                    { new Guid("e57ed7c0-4cc5-4d12-a88b-ed9f2997d918"), "Colnago" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "Description", "Price", "Stock", "Title" },
                values: new object[,]
                {
                    { new Guid("7201fd50-25b9-4b7d-99a7-b367b73222f8"), new Guid("cd3eb3f1-0143-495b-9b90-9d1e8e46fbad"), "High-End aero bike for the flats", 10000.0, 100, "Madone" },
                    { new Guid("ab0f5a1f-9b48-4862-8e6a-bced8d20558e"), new Guid("e29de237-8203-4e3e-8066-4ac71d2c707f"), "For the mountains", 11000.0, 100, "Vam" },
                    { new Guid("d4b1d999-862d-4cf9-bcb7-b79de08768b9"), new Guid("e57ed7c0-4cc5-4d12-a88b-ed9f2997d918"), "Made for winning", 12000.0, 100, "V4Rs" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
