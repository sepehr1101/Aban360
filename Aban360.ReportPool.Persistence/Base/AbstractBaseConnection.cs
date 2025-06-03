using Aban360.Common.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class AbstractBaseConnection
    {
        private readonly IConfiguration _configuration;
        protected AbstractBaseConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.NotNull(nameof(configuration));
        }
        internal SqlConnection _sqlConnection
        {
            get
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                connectionString.NotEmptyString(nameof(connectionString));
                SqlConnection sqlConnection = new(connectionString);
                return sqlConnection;
            }
        }

        internal SqlConnection _sqlReportConnection
        {
            get
            {
                string connectionString = _configuration.GetConnectionString("ReportConnection");
                connectionString.NotEmptyString(nameof(connectionString));
                SqlConnection sqlConnection = new(connectionString);
                return sqlConnection;
            }
        }
    }
}
