using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class ImprovedShoppingCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ShoppingCar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TotalPrice",
                table: "ShoppingCar",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
