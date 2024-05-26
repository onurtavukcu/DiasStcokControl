using DiasDataAccessLayer.EntityDbContext;
using Microsoft.EntityFrameworkCore;

namespace DiasAPI.CheckDbExtension
{
    public static class CheckDbExistance
    {
        public static IServiceCollection AddAndMigrateDatabase(this IServiceCollection services)
        {
            using IServiceScope CurrentScope = services.BuildServiceProvider().CreateScope();
            var dbContext = CurrentScope.ServiceProvider.GetService<ProductDbContext>();

            if (dbContext.Database.EnsureCreated())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Create and Migrate Database");
                Console.ResetColor();
                dbContext.Database.Migrate();
            }

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Migrate Database");
                Console.ResetColor();
                dbContext.Database.Migrate();
            }

            return services;
        }
    }
}