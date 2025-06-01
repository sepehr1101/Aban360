using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class CustomerSearchAdvancedQueryService : AbstractBaseConnection, ICustomerSearchAdvancedQueryService
    {
        public CustomerSearchAdvancedQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ICollection<CustomerSearchOutputDto>> GetInfo(CustomerSearchAdvancedInputDto input)
        {
            string customerSearchInfoQuery = GetCustomerSearchInfo();
            IEnumerable<CustomerSearchOutputDto> results = await _sqlConnection.QueryAsync<CustomerSearchOutputDto>(customerSearchInfoQuery);//todo: send parameters

            return results.ToList();
        }

        private string GetCustomerSearchInfo()
        {
            return " ";
        }
    }
}
