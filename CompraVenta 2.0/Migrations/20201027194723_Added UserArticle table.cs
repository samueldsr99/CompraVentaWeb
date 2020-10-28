using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class AddedUserArticletable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_ShoppingCar_ShoppingCarId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ShoppingCarId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ShoppingCarId",
                table: "Articles");

            migrationBuilder.CreateTable(
                name: "UserArticle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    ArticleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserArticle", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserArticle");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCarId",
                table: "Articles",
                nullable: true);

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
    }
}
