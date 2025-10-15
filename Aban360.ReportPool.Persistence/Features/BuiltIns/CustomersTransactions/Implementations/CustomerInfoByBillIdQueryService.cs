using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class CustomerInfoByBillIdQueryService : AbstractBaseConnection, ICustomerInfoByBillIdQueryService
    {
        public CustomerInfoByBillIdQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<CustomerInfoByBillIdOutputDto> Get(string billId)
        {
            string customerInfoByBillIdQueryString = GetCustomerInfoByBillIdQuery();
            CustomerInfoByBillIdOutputDto customerInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerInfoByBillIdOutputDto>(customerInfoByBillIdQueryString, new { billId });
            return customerInfo;
        }

        private string GetCustomerInfoByBillIdQuery()
        {
            return @"Select
                    	c.CustomerNumber,
                    	c.ReadingNumber
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.BillId=@billId";
        }
    }
}
