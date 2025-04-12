using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using Aban360.Common.Categories;
using Aban360.Common.Db.DbSeeder.Contracts;

namespace Aban360.Common.Db.Extensions
{
    public static class MigrationRunner
    {
        public static void UpdateAndSeedDb(this IServiceCollection services)
        {
            Tuple<string, DatabaseCreationParameters?,bool> connectionInfo = GetConnectionInfo();
            services.UpdateAndSeedDb(connectionInfo.Item1, connectionInfo.Item3 ? null : connectionInfo.Item2);
        }
        private static void UpdateAndSeedDb(this IServiceCollection services, string connectionString, DatabaseCreationParameters? databaseCreationParameters, [Optional] string dbName)
        {
            using (var serviceProvider = CreateServices(services, connectionString))
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    EnsureDatabase(connectionString, databaseCreationParameters, dbName);
                    UpdateDatabase(scope.ServiceProvider);
                    SeedDatabse(scope.ServiceProvider);
                }
            }
        }
        private static ServiceProvider CreateServices(IServiceCollection services, string connectionString)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                 .Where(assembly => assembly.GetName().Name.Contains("Persistence"))
                 .ToArray();
            return services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddSqlServer()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(assemblies).For.All())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void EnsureDatabase(string connectionString, [Optional] DatabaseCreationParameters? databaseCreationParameters, [Optional] string dbName)
        {
            var connectionBuilder = new SqlConnectionStringBuilder(connectionString);
            var initialCatalog = string.IsNullOrWhiteSpace(dbName) ? connectionBuilder.InitialCatalog : dbName;

            connectionBuilder.InitialCatalog = "master";
            using var connection = new SqlConnection(connectionBuilder.ConnectionString);
            string configureFilestreamQuery = GetConfigureFilestreamQuery();
            string createDbQuery =
                $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{initialCatalog}') " +
                $"BEGIN " +
                $"CREATE DATABASE {initialCatalog} " +
                $"{GetDbCratetionParametersQuery(databaseCreationParameters)} " +
                $"END";

            SqlCommand createDbCommand = new SqlCommand(createDbQuery, connection);
            SqlCommand configureDbCommand = new SqlCommand(configureFilestreamQuery, connection);
            try
            {
                connection.Open();
                configureDbCommand.ExecuteNonQuery();
                createDbCommand.ExecuteNonQuery();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        private static string GetConfigureFilestreamQuery()
        {
            string query = @"
                IF NOT EXISTS (SELECT 1 FROM sys.configurations WHERE name = 'filestream access level' AND value_in_use = 2)
                BEGIN
                    EXEC sp_configure 'filestream access level', 2;
                    RECONFIGURE;
                END";
            return query;
        }
        private static string GetDbCratetionParametersQuery(DatabaseCreationParameters? databaseCreationParameters)
        {           
            if (databaseCreationParameters is null)
            {
                return string.Empty;
            }
            string formattedDay = DateTime.Now.ToString("yyyy-MM-dd-HH-ss");
            string query =
                 $"ON PRIMARY " +
                     $"(NAME = {databaseCreationParameters.MdfName}, " +
                     $"FILENAME = '{databaseCreationParameters.MdfFileName}', " +
                     $"SIZE = {databaseCreationParameters.MdfSize}, " +
                     $"MAXSIZE = {databaseCreationParameters.MdfMaxSize}, " +
                     $"FILEGROWTH = {databaseCreationParameters.MdfFileGrowth}), " +
                 $"FILEGROUP FileStreamGroup CONTAINS FILESTREAM " +
                     $"(NAME = {databaseCreationParameters.StreamName}, " +
                     $"FILENAME = '{databaseCreationParameters.StreamFileName}\\{formattedDay}' ) " +
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
            IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
        private static void SeedDatabse(IServiceProvider serviceProvider)
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
                    StreamName = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:StreamName")?.Value,
                    MdfFileName = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:MdfFileName")?.Value,
                    LdfFileName = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:LdfFileName")?.Value,
                    StreamFileName = configuration.GetSection($"{nameof(DatabaseCreationParameters)}:StreamFileName")?.Value,
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
