using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class brandrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrand_Brands_BrandsId",
                table: "ProductBrand");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrand_BrandsId",
                table: "ProductBrand");

            migrationBuilder.DropColumn(
                name: "BrandsId",
                table: "ProductBrand");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrand_BrandID",
                table: "ProductBrand",
                column: "BrandID");

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_ProductBrand",
                table: "ProductBrand",
                column: "BrandID",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_ProductBrand",
                table: "ProductBrand");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrand_BrandID",
                table: "ProductBrand");

            migrationBuilder.AddColumn<Guid>(
                name: "BrandsId",
                table: "ProductBrand",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrand_BrandsId",
                table: "ProductBrand",
                column: "BrandsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrand_Brands_BrandsId",
                table: "ProductBrand",
                column: "BrandsId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
