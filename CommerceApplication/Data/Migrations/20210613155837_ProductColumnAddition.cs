using Microsoft.EntityFrameworkCore.Migrations;

namespace CommerceApplication.Data.Migrations
{
    public partial class ProductColumnAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                schema: "Ecommerce",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                schema: "Ecommerce",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                schema: "Ecommerce",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                schema: "Ecommerce",
                table: "Products",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                schema: "Ecommerce",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                schema: "Ecommerce",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                schema: "Ecommerce",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                schema: "Ecommerce",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
