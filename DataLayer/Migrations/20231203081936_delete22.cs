using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class delete22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Blogs_BlogID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Products_ProductID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeProduct_AspNetUsers_UserId",
                table: "LikeProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_LikeProduct_Products_ProductID",
                table: "LikeProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_OrderID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Features_Features_FeatureID",
                table: "Product_Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Features_Products_ProductID",
                table: "Product_Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Galleries_Products_ProductID",
                table: "Product_Galleries");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Groups_Product_Groups_ParentID",
                table: "Product_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Selected_Groups_Product_Groups_GroupID",
                table: "Product_Selected_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Selected_Groups_Products_ProductID",
                table: "Product_Selected_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Tags_Products_ProductID",
                table: "Product_Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrand_Products_ProductID",
                table: "ProductBrand");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Addresses",
                table: "Addresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Comments",
                table: "Comments",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Comments",
                table: "Comments",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_LikeProduct",
                table: "LikeProduct",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LikeProduct",
                table: "LikeProduct",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_OrderDetails",
                table: "OrderDetails",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Product_Features",
                table: "Product_Features",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Product_Galleries",
                table: "Product_Galleries",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Groups_Product_Groups",
                table: "Product_Groups",
                column: "ParentID",
                principalTable: "Product_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Groups_Product_Selected_Groups",
                table: "Product_Selected_Groups",
                column: "GroupID",
                principalTable: "Product_Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Product_Selected_Groups",
                table: "Product_Selected_Groups",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Product_Tags",
                table: "Product_Tags",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductBrand",
                table: "ProductBrand",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Addresses",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Comments",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Comments",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_LikeProduct",
                table: "LikeProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_LikeProduct",
                table: "LikeProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Product_Features",
                table: "Product_Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Product_Galleries",
                table: "Product_Galleries");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Groups_Product_Groups",
                table: "Product_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Groups_Product_Selected_Groups",
                table: "Product_Selected_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Product_Selected_Groups",
                table: "Product_Selected_Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Product_Tags",
                table: "Product_Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductBrand",
                table: "ProductBrand");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Blogs_BlogID",
                table: "Comments",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Products_ProductID",
                table: "Comments",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeProduct_AspNetUsers_UserId",
                table: "LikeProduct",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LikeProduct_Products_ProductID",
                table: "LikeProduct",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_OrderID",
                table: "OrderDetails",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Features_Features_FeatureID",
                table: "Product_Features",
                column: "FeatureID",
                principalTable: "Features",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Features_Products_ProductID",
                table: "Product_Features",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Galleries_Products_ProductID",
                table: "Product_Galleries",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Groups_Product_Groups_ParentID",
                table: "Product_Groups",
                column: "ParentID",
                principalTable: "Product_Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Selected_Groups_Product_Groups_GroupID",
                table: "Product_Selected_Groups",
                column: "GroupID",
                principalTable: "Product_Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Selected_Groups_Products_ProductID",
                table: "Product_Selected_Groups",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Tags_Products_ProductID",
                table: "Product_Tags",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrand_Products_ProductID",
                table: "ProductBrand",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
