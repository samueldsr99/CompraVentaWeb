using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class ModifiedAuctionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AName",
                table: "Auctions");

            migrationBuilder.RenameColumn(
                name: "ACategory",
                table: "Auctions",
                newName: "ArticleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Auctions",
                newName: "ACategory");

            migrationBuilder.AddColumn<string>(
                name: "AName",
                table: "Auctions",
                nullable: true);
        }
    }
}
