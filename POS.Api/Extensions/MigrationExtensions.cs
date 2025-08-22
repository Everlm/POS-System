using Microsoft.EntityFrameworkCore;
using POS.Infrastructure.Persistences.Contexts;

namespace POS.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<POSContext>();

            dbContext.Database.Migrate();
        }
    }
}