using Aban360.PaymentPool.Persistence.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Aban360.PaymentPool.Persistence.Dapper
{
    internal abstract class AbstractPaymentPoolConnection
    {
        private readonly string _connectionString;
        protected AbstractPaymentPoolConnection(IConfiguration configuration)
        {
            //_connectionString = PaymentPoolDatabaseSettings.GetConnectionString(configuration);
        }
        protected SqlConnection _sqlConnection => new(_connectionString);
    }
}
