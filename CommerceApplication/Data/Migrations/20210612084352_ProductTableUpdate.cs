using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CommerceApplication.Data.Migrations
{
    public partial class ProductTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProductImageData",
                schema: "Ecommerce",
                table: "Products",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImageData",
                schema: "Ecommerce",
                table: "Products");
        }
    }
}
