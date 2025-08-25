using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;

namespace POS.Infrastructure.SeedData
{
    public static class POSContextSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Password = BC.HashPassword("123456"),
                    Email = "admin@sistema.com",
                    Image = null,
                    AuthType = "Interno",
                    State = 1,
                    AuditCreateUser = 1,
                    AuditCreateDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    Description = "Admin"
                },
                new Role
                {
                    RoleId = 2,
                    Description = "General"
                }
            );
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    UserRoleId = -1,
                    UserId = 1,
                    RoleId = 1
                },
                new UserRole
                {
                    UserRoleId = -2,
                    UserId = 1,
                    RoleId = 2
                }
            );
        }
    }
}
