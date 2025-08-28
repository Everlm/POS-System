using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Persistences.Migrations
{
    public partial class SeddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "AuditCreateDate", "AuditCreateUser", "AuditDeleteDate", "AuditDeleteUser", "AuditUpdateDate", "AuditUpdateUser", "Description", "Name", "State" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, null, null, null, "Bebidas", null, 0 },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, null, null, null, null, "Comidas", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "State" },
                values: new object[,]
                {
                    { 1, "Admin", null },
                    { 2, "General", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "AuditCreateDate", "AuditCreateUser", "AuditDeleteDate", "AuditDeleteUser", "AuditUpdateDate", "AuditUpdateUser", "AuthType", "Email", "Image", "Password", "RefreshToken", "RefreshTokenExpiryTime", "State", "UserName" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, null, null, null, null, "Interno", "admin@sistema.com", null, "$2a$11$ufCETAI26jewwMP0YrD2s.BQ2HSfsUghSKxsVq3czMlMaIYVBwt6G", null, null, 1, "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleId", "BranchOfficeId", "RoleId", "State", "UserId" },
                values: new object[] { -2, null, 2, null, 1 });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "UserRoleId", "BranchOfficeId", "RoleId", "State", "UserId" },
                values: new object[] { -1, null, 1, null, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "UserRoleId",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);
        }
    }
}
