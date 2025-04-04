using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Persistences.Migrations
{
    public partial class Alter_Entity_PurcharseDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPurchasePrice",
                table: "PurcharseDetails",
                newName: "UnitPurcharsePrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPurcharsePrice",
                table: "PurcharseDetails",
                newName: "UnitPurchasePrice");
        }
    }
}
