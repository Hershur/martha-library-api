using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Infra
{
    public static class MigrateContext
    {
        public static void RunMigration(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope()){
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();
                
                if (dbContext.Database.GetPendingMigrations().Any()) {
                    dbContext.Database.Migrate();
                }
            }
        }
    }
}