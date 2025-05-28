using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Persistences.Migrations
{
    public partial class AlterVoucherDocumentTypeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_VoucherDoumentType_VoucherDoumentTypeId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_VoucherDoumentTypeId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "VoucherDoumentTypeId",
                table: "Sales");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_VoucherDocumentTypeId",
                table: "Sales",
                column: "VoucherDocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_VoucherDoumentType_VoucherDocumentTypeId",
                table: "Sales",
                column: "VoucherDocumentTypeId",
                principalTable: "VoucherDoumentType",
                principalColumn: "VoucherDoumentTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_VoucherDoumentType_VoucherDocumentTypeId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_VoucherDocumentTypeId",
                table: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "VoucherDoumentTypeId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_VoucherDoumentTypeId",
                table: "Sales",
                column: "VoucherDoumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_VoucherDoumentType_VoucherDoumentTypeId",
                table: "Sales",
                column: "VoucherDoumentTypeId",
                principalTable: "VoucherDoumentType",
                principalColumn: "VoucherDoumentTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
