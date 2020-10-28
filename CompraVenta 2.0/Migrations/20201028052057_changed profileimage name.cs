using Microsoft.EntityFrameworkCore.Migrations;

namespace CompraVenta.Migrations
{
    public partial class changedprofileimagename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "profileImagePath",
                table: "AspNetUsers",
                newName: "ProfileImagePath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfileImagePath",
                table: "AspNetUsers",
                newName: "profileImagePath");
        }
    }
}
