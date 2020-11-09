using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class AddedSoldfieldtoAuction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "Auctions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Auctions");
        }
    }
}
