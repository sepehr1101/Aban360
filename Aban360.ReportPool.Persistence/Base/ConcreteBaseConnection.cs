using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal class ConcreteBaseConnection : AbstractBaseConnection
    {
        public ConcreteBaseConnection(IConfiguration configuration)
            : base(configuration)
        {
        }

        public SqlConnection GetConnection() => _sqlConnection;
        public string GetConnectionString() => _sqlConnection.ConnectionString;
    }

}
