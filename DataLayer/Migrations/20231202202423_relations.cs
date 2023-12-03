using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UsersId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Blogs_BlogsId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Products_ProductsId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeProduct_AspNetUsers_UsersId",
                table: "LikeProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeProduct_Products_ProductsId",
                table: "LikeProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrdersId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Features_Features_FeaturesId",
                table: "Product_Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Features_Products_ProductsId",
                table: "Product_Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Groups_Product_Groups_Product_GroupsId",
                table: "Product_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Selected_Groups_Product_Groups_Product_GroupsId",
                table: "Product_Selected_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Selected_Groups_Products_ProductsId",
                table: "Product_Selected_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Tags_Products_ProductsId",
                table: "Product_Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrand_Products_ProductsId",
                table: "ProductBrand");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrand_ProductsId",
                table: "ProductBrand");

            migrationBuilder.DropIndex(
                name: "IX_Product_Tags_ProductsId",
                table: "Product_Tags");

            migrationBuilder.DropIndex(
                name: "IX_Product_Selected_Groups_Product_GroupsId",
                table: "Product_Selected_Groups");

            migrationBuilder.DropIndex(
                name: "IX_Product_Selected_Groups_ProductsId",
                table: "Product_Selected_Groups");

            migrationBuilder.DropIndex(
                name: "IX_Product_Groups_Product_GroupsId",
                table: "Product_Groups");

            migrationBuilder.DropIndex(
                name: "IX_Product_Features_FeaturesId",
                table: "Product_Features");

            migrationBuilder.DropIndex(
                name: "IX_Product_Features_ProductsId",
                table: "Product_Features");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_OrdersId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_LikeProduct_ProductsId",
                table: "LikeProduct");

            migrationBuilder.DropIndex(
                name: "IX_LikeProduct_UsersId",
                table: "LikeProduct");

            migrationBuilder.DropIndex(
                name: "IX_Comments_BlogsId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ProductsId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UsersId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "ProductBrand");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Product_Tags");

            migrationBuilder.DropColumn(
                name: "Product_GroupsId",
                table: "Product_Selected_Groups");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Product_Selected_Groups");

            migrationBuilder.DropColumn(
                name: "Product_GroupsId",
                table: "Product_Groups");

            migrationBuilder.DropColumn(
                name: "FeaturesId",
                table: "Product_Features");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Product_Features");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "LikeProduct");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "LikeProduct");

            migrationBuilder.DropColumn(
                name: "BlogsId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Addresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "ProductBrand",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "Product_Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Product_GroupsId",
                table: "Product_Selected_Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "Product_Selected_Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Product_GroupsId",
                table: "Product_Groups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "FeaturesId",
                table: "Product_Features",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "Product_Features",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrdersId",
                table: "OrderDetails",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "LikeProduct",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UsersId",
                table: "LikeProduct",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BlogsId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UsersId",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrand_ProductsId",
                table: "ProductBrand",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Tags_ProductsId",
                table: "Product_Tags",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Selected_Groups_Product_GroupsId",
                table: "Product_Selected_Groups",
                column: "Product_GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Selected_Groups_ProductsId",
                table: "Product_Selected_Groups",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Groups_Product_GroupsId",
                table: "Product_Groups",
                column: "Product_GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Features_FeaturesId",
                table: "Product_Features",
                column: "FeaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Features_ProductsId",
                table: "Product_Features",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrdersId",
                table: "OrderDetails",
                column: "OrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeProduct_ProductsId",
                table: "LikeProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeProduct_UsersId",
                table: "LikeProduct",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogsId",
                table: "Comments",
                column: "BlogsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductsId",
                table: "Comments",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UsersId",
                table: "Addresses",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UsersId",
                table: "Addresses",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Blogs_BlogsId",
                table: "Comments",
                column: "BlogsId",
                principalTable: "Blogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Products_ProductsId",
                table: "Comments",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeProduct_AspNetUsers_UsersId",
                table: "LikeProduct",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeProduct_Products_ProductsId",
                table: "LikeProduct",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrdersId",
                table: "OrderDetails",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Features_Features_FeaturesId",
                table: "Product_Features",
                column: "FeaturesId",
                principalTable: "Features",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Features_Products_ProductsId",
                table: "Product_Features",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Groups_Product_Groups_Product_GroupsId",
                table: "Product_Groups",
                column: "Product_GroupsId",
                principalTable: "Product_Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Selected_Groups_Product_Groups_Product_GroupsId",
                table: "Product_Selected_Groups",
                column: "Product_GroupsId",
                principalTable: "Product_Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Selected_Groups_Products_ProductsId",
                table: "Product_Selected_Groups",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Tags_Products_ProductsId",
                table: "Product_Tags",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrand_Products_ProductsId",
                table: "ProductBrand",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
