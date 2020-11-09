using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class RemovedShoppingCarTableChangeUserArticleNametoShoppingCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserArticle");

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "ShoppingCar",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "ShoppingCar");

            migrationBuilder.CreateTable(
                name: "UserArticle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArticleId = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserArticle", x => x.Id);
                });
        }
    }
}
