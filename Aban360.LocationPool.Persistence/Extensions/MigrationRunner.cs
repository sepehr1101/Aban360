using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using Aban360.Common.Categories;
using Aban360.Common.Db.DbSeeder.Contracts;
z
namespace Aban360.LocationPool.Persistence.Extensions
{
    public static class MigrationRunner
    {
        public static void UpdateAndSeedLocationPoolDb(this IServiceCollection services)
        {
            var connectionInfo = GetConnectionInfo();
            services.UpdateAndSeedDb(connectionInfo.Item1, connectionInfo.Item3 ? null : connectionInfo.Item2);
        }
        private static void UpdateAndSeedDb(this IServiceCollection services, string connectionString, DatabaseCreationParameters? databaseCreationParameters, [Optional] string dbName)
        {
            //using (var serviceProvider = CreateServices(services, connectionString))
            //{
            //    using (var scope = serviceProvider.CreateScope())
            //    {
            //        EnsureDatabase(connectionString, databaseCreationParameters, dbName);
            //        UpdateDatabase(scope.ServiceProvider);
            //        SeedDatabse(scope.ServiceProvider);
            //    }
            //}
        }
        private static ServiceProvider CreateServices(IServiceCollection services, string connectionString)
        {
            return services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        public static void EnsureDatabase(string connectionString, [Optional] DatabaseCreationParameters? databaseCreationParameters, [Optional] string dbName)
        {
            var connectionBuilder = new SqlConnectionStringBuilder(connectionString);
            var initialCatalog = string.IsNullOrWhiteSpace(dbName) ? connectionBuilder.InitialCatalog : dbName;

            connectionBuilder.InitialCatalog = "master";
            using var connection = new SqlConnection(connectionBuilder.ConnectionString);
            var str =
                $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{initialCatalog}') " +
                $"BEGIN " +
                $"CREATE DATABASE {initialCatalog} " +
                $"{GetDbCratetionParametersQuery(databaseCreationParameters)} " +
                $"END";

            SqlCommand myCommand = new SqlCommand(str, connection);
            try
            {
                connection.Open();
                myCommand.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private static string GetDbCratetionParametersQuery(DatabaseCreationParameters? databaseCreationParameters)
        {
            if (databaseCreationParameters is null)
            {
                return string.Empty;
            }
            string query =
                 $"ON PRIMARY " +
                     $"(NAME = {databaseCreationParameters.MdfName}, " +
                     $"FILENAME = '{databaseCreationParameters.MdfFileName}', " +
                     $"SIZE = {databaseCreationParameters.MdfSize}, " +
                     $"MAXSIZE = {databaseCreationParameters.MdfMaxSize}, " +
                     $"FILEGROWTH = {databaseCreationParameters.MdfFileGrowth}) " +
                 $"LOG ON " +
                     $"(NAME = {databaseCreationParameters.LdfName}, " +
                     $"FILENAME = '{databaseCreationParameters.LdfFileName}', " +
                     $"SIZE = {databaseCreationParameters.LdfSize}, " +
                     $"MAXSIZE = {databaseCreationParameters.LdfMaxSize}, " +
                     $"FILEGROWTH = {databaseCreationParameters.LdfFileGrowth}) ";
            return query;
        }
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();

        }
        public static void SeedDatabse(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IDataSeedersRunner>();
            runner.RunAllDataSeeders();
        }
        public static Tuple<string, DatabaseCreationParameters?, bool> GetConnectionInfo()
        {
            var basePath = Directory.GetCurrentDirectory();
            var configuration = new ConfigurationBuilder()
                                    .SetBasePath(basePath)
                                    .AddJsonFile("appsettings.json")
                                    .Build();
            bool isIntegrationTest = Convert.ToBoolean(configuration.GetSection($"IsIntegrationTest")?.Value);
            var connectionString = isIntegrationTest ?
                configuration.GetConnectionString("DockerTestConnection") :
                configuration.GetConnectionString("DefaultConnection");
            DatabaseCreationParameters? databaseCreationParameters = isIntegrationTest ?
                null :
                GetDbCreationParameters(configuration);
            return new Tuple<string, DatabaseCreationParameters?, bool>(connectionString, databaseCreationParameters, isIntegrationTest);
        }

        private static DatabaseCreationParameters? GetDbCreationParameters(IConfigurationRoot configuration)
        {
            try
            {
                return new DatabaseCreationParameters()
                {
                    MdfName = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:MdfName")?.Value,
                    LdfName = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:LdfName")?.Value,
                    MdfFileName = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:MdfFileName")?.Value,
                    LdfFileName = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:LdfFileName")?.Value,
                    MdfSize = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:MdfSize")?.Value,
                    LdfSize = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:LdfSize")?.Value,
                    MdfMaxSize = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:MdfMaxSize")?.Value,
                    LdfMaxSize = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:LdfMaxSize")?.Value,
                    MdfFileGrowth = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:MdfFileGrowth")?.Value,
                    LdfFileGrowth = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:LdfFileGrowth")?.Value,
                };
            }
            catch
            {
                return default;
            }
        }
    }
}
