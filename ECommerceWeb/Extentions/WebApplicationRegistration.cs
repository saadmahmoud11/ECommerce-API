using ECommerece.Domain.Contracts;
using ECommerece.presistance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWeb.Extentions;

public static class WebApplicationRegistration
{
    public async static Task <WebApplication> MigrateDatabaesAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContextService = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
        var pendingMigrations = await dbContextService.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
           await dbContextService.Database.MigrateAsync();
        }
        return app;
    }

    public async static Task <WebApplication> SeedDataAsync( this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dataInitializerService = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
        await dataInitializerService.InitializeAsync();
        return app;
    }
}
