using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class Addedshoppingcar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShoppingCarId",
                table: "Articles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShoppingCar",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    TotalPrice = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ShoppingCarId",
                table: "Articles",
                column: "ShoppingCarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_ShoppingCar_ShoppingCarId",
                table: "Articles",
                column: "ShoppingCarId",
                principalTable: "ShoppingCar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ShoppingCar_ShoppingCarId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "ShoppingCar");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ShoppingCarId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ShoppingCarId",
                table: "Articles");
        }
    }
}
