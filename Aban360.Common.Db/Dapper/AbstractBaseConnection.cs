using Aban360.Common.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.Common.Db.Dapper
{
    public  abstract class AbstractBaseConnection
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
    }
}
