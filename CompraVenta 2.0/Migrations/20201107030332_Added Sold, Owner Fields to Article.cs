using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class AddedSoldOwnerFieldstoArticle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "Articles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Articles");
        }
    }
}
