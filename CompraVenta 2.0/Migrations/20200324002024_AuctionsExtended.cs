using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class AuctionsExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentOwner",
                table: "Auctions",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CurrentPrice",
                table: "Auctions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentOwner",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "Auctions");
        }
    }
}
