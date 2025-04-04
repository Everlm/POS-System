using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Persistences.Migrations
{
    public partial class Alter_Entity_ProductStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockId",
                table: "ProductStock");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "ProductStock",
                newName: "PurcharsePrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurcharsePrice",
                table: "ProductStock",
                newName: "PurchasePrice");

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "ProductStock",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
