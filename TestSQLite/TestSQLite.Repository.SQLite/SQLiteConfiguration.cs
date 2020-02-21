using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestSQLite.Repository.SQLite
{
    public static class SQLiteConfiguration
    {
        public static string GetConnectionString(string databaseName) => $"Data Source={databaseName}.db";

        public static void AddDefaultConfigureServiceSQLite(this IServiceCollection services, string databaseName = "SQLiteDefaultDabase")
        {
            services.AddDbContext<SQLiteContext>(options =>
                options.UseSqlite(GetConnectionString(databaseName))
            );
        }

        public static void UpdateDatabase(IServiceScopeFactory serviceScopeFactory)
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<SQLiteContext>();
            context.Database.Migrate();
        }
    }
}
