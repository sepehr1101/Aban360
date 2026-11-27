using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.PaymentPool.Persistence.Configuration
{
    /*public static class PaymentPoolDatabaseSettings
    {
        public const string AssemblyName = "Aban360.PaymentPool.Persistence";
        public static bool ManualMigrations(IConfiguration configuration)
        {
            var value = configuration["PaymentPool:ManualMigrations"];
            if (string.IsNullOrWhiteSpace(value)) return false;
            return bool.TryParse(value, out var manual) ? manual
                : throw new InvalidOperationException("PaymentPool:ManualMigrations must be true or false.");
        }
        public static string GetConnectionString(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("PaymentPoolConnection");
            if (ManualMigrations(configuration))
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Configure ConnectionStrings:PaymentPoolConnection for the Atlas server before starting the application.");
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (!string.Equals(builder.InitialCatalog, "Atlas", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("PaymentPoolConnection must target the Atlas database in manual migration mode.");
            }
            else if (!string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException("Enable PaymentPool:ManualMigrations when configuring PaymentPoolConnection; the shared migration runner cannot migrate a second server.");
            else
                connectionString = configuration.GetConnectionString("DefaultConnection");
            return !string.IsNullOrWhiteSpace(connectionString) ? connectionString
                : throw new InvalidOperationException("The PaymentPool database connection is not configured.");
        }
    }*/
}
