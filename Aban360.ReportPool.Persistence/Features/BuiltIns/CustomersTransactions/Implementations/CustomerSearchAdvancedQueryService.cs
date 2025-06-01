using Aban360.ReportPool.Domain.Base;
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

        public async Task<ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>> GetInfo(CustomerSearchAdvancedInputDto input)
        {
            string customerSearchDataInfoQuery = GetCustomerSearchDataQuery();
            IEnumerable<CustomerSearchDataOutputDto> customerData = await _sqlConnection.QueryAsync<CustomerSearchDataOutputDto>(customerSearchDataInfoQuery);//todo: send parameters
            CustomerSearchHeaderOutputDto customerHeader = new CustomerSearchHeaderOutputDto()
            { };

            var result = new ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>(ReportLiterals.CustomerSearch, customerHeader, customerData);
            return result;
        }

        private string GetCustomerSearchDataQuery()
        {
            return " ";
        }
    }
}
