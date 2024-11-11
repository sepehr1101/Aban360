using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using System.Reflection;
using Aban360.UserPool.Persistence.DbSeeder.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Aban360.UserPool.Persistence.Extensions
{
    public static class MigrationRunner
    {
        public static void UpdateAndSeedDb(this IServiceCollection services, string connectionString)
        {
            using (var serviceProvider = CreateServices(services, connectionString))
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    UpdateDatabase(scope.ServiceProvider);
                    SeedDatabse(scope.ServiceProvider);
                }
            }
        }
        private static ServiceProvider CreateServices(IServiceCollection services, string connectionString)
        {            
            var tmpConnectionString = GetConnectionString();
            return services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(tmpConnectionString)
                    //.ScanIn(typeof(DbInitialDesign).Assembly).For.Migrations())
                    .ScanIn(Assembly.GetExecutingAssembly()).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();

        }
        public static void SeedDatabse(IServiceProvider serviceProvider)
        {
            var runner= serviceProvider.GetRequiredService<IDataSeedersRunner>();
            runner.RunAllDataSeeders();
        }
        private static string? GetConnectionString()
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(basePath)
                                    .AddJsonFile("appsettings.json")
                                    .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            return connectionString;
        }
    }
}
