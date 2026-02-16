using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Domain.Features.Transactions;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class CustomerInfoQueryService : AbstractBaseConnection, ICustomerInfoQueryService
    {
        public CustomerInfoQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<CustomerInfoByBillIdOutputDto> Get(string billId)
        {
            string query = GetCustomerInfoByBillIdQuery();
            CustomerInfoByBillIdOutputDto customerInfo = await _sqlReportConnection.QueryFirstOrDefaultAsync<CustomerInfoByBillIdOutputDto>(query, new { billId });
            return customerInfo;
        }
        public async Task<BillIdReppar> Get(CustomerInfoByZoneAndCustomerNumberInputDto input)
        {
            string query = GetBillIdByZoneIdAndCustomerNumberQuery();
            BillIdReppar billIdRepper = await _sqlReportConnection.QueryFirstOrDefaultAsync<BillIdReppar>(query, new { zoneId = input.ZoneId, customerNumber = input.CustomerNumber });
            if (billIdRepper is null || string.IsNullOrWhiteSpace(billIdRepper.BillId))
            {
                throw new InvalidBillIdException(ExceptionLiterals.BillIdNotFound);
            }

            return billIdRepper;
        }
        public async Task<ZoneIdAndCustomerNumberOutputDto> GetZoneIdAndCustomerNumber(string billId)
        {
            string query = GetZoneIdAndCustomerNumberByBillIdQuery();
            ZoneIdAndCustomerNumberOutputDto result = await _sqlReportConnection.QueryFirstOrDefaultAsync<ZoneIdAndCustomerNumberOutputDto>(query, new { billId });
            if (result is null || result.ZoneId <= 0)
            {
                throw new InvalidBillIdException(ExceptionLiterals.InvalidBillId);
            }

            return result;
        }


        private string GetBillIdByZoneIdAndCustomerNumberQuery()
        {
            return @"Select c.BillId
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.ZoneId=@zoneId AND
                    	c.CustomerNumber=@customerNumber";
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
        private string GetZoneIdAndCustomerNumberByBillIdQuery()
        {
            return @"Select 
                		ZoneId,
                		CustomerNumber
                	From CustomerWarehouse.dbo.Clients
                	Where	
                		BillId=@billId AND
                		ToDayJalali IS NULL";
        }
    }
}
