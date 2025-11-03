using Aban360.Common.Extensions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Aban360.Common.Db.Dapper
{
    public abstract class AbstractBaseConnection
    {
        private readonly IConfiguration _configuration;
        protected AbstractBaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));
        }
        public SqlConnection _sqlConnection
        {
            get
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                connectionString.NotEmptyString(nameof(connectionString));
                SqlConnection sqlConnection = new(connectionString);
                return sqlConnection;
            }
        }

        public SqlConnection _sqlReportConnection
        {
            get
            {
                string connectionString = _configuration.GetConnectionString("ReportConnection");
                connectionString.NotEmptyString(nameof(connectionString));
                SqlConnection sqlConnection = new(connectionString);
                return sqlConnection;
            }
        }
        
        public string GetDbName(int zoneId)
        {
            return zoneId > 140000 ? "Abfar" : zoneId.ToString();
        }

        public int GetMergedZoneId(int zoneId)
        {
            return zoneId > 140000 ? zoneId - 10000 : zoneId;
        }
        public async Task ExecuteBatchAsync(SqlConnection sqlConnection, string sqlFilePath)
        {
            if (!File.Exists(sqlFilePath))
                throw new FileNotFoundException($"SQL file not found: {sqlFilePath}");

            string sqlScript = File.ReadAllText(sqlFilePath);

            var sqlCommands = sqlScript
                .Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries)//"GO", "Go", "go"
                .Select(cmd => cmd.Trim())
                .Where(cmd => !string.IsNullOrWhiteSpace(cmd) && !cmd.Contains("IDENTITY_INSERT"))
                .ToList();

            int counter = 0;

            using (var connection = sqlConnection)
            {
                await connection.OpenAsync();

                foreach (var command in sqlCommands)
                {
                    try
                    {
                        await connection.ExecuteAsync(command);
                        counter++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Error executing command #{counter + 1}:\n{ex.Message}\nCommand:\n{command}\n");
                        throw;
                    }
                }
            }

            Console.WriteLine($"✅ {counter} SQL commands executed successfully.");
        }

    }
}
