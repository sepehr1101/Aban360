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
        private int _lagalNationalCodeCharecter = 11;
        private int _naturalNationalCodeCharecter = 10;
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
        public async Task<IEnumerable<CustomerLegalDetailDataOutputDto>> GetDetail(CustomerLegalInputDto input)
        {
            string query = GetLegalsInfoDetailQuery();
            IEnumerable<CustomerLegalDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<CustomerLegalDetailDataOutputDto>(query, input);
            return data;
        }
        public async Task<IEnumerable<CustomerLegalSummaryDataOutputDto>> GetSummary(CustomerLegalInputDto input)
        {
            string query = GetLegalsInfoSummaryQuery();
            IEnumerable<CustomerLegalSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<CustomerLegalSummaryDataOutputDto>(query, input);
            return data;
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
        private string GetLegalsInfoDetailQuery()
        {
            return @$"Select 
                    	t51.C0 ZoneId,
                    	t51.C2 ZoneTitle,
                    	t46.C0 RegionId,
                    	t46.C2 RegionTitle,
                    	c.BillId,
                    	c.FirstName + ' ' + c.SureName FullName,
                    	c.UsageId,
                    	c.UsageTitle,
                    	c.MobileNo MobileNumber,
                    	c.PhoneNo PhoneNumber,
                    	c.NationalId NationalCode
                    From CustomerWarehouse.dbo.Clients c
                    Join [Db70].dbo.T51 t51
                    	ON t51.C0=c.ZoneId
                    Join [Db70].dbo.T46 t46
                    	ON t46.C0=t51.C1
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.ZoneId IN @ZoneIds AND
                    	c.NationalId IS NOT NULL AND
                    	LEN(c.NationalId)={_lagalNationalCodeCharecter}";
        }
        private string GetLegalsInfoSummaryQuery()
        {
            return @$"Select 
                    	MAX(c.ZoneId) ZoneId,
                    	c.ZoneTitle,
                    	COUNT(CASE WHEN c.NationalId IS NOT NULL AND LEN(TRIM(c.NationalId))={_naturalNationalCodeCharecter} THEN 1 ELSE null END ) NaturalCount,
                    	COUNT(CASE WHEN c.NationalId IS NOT NULL AND LEN(TRIM(c.NationalId))={_lagalNationalCodeCharecter} THEN 1 ELSE null END ) LegalCount,
                    	COUNT(CASE WHEN c.NationalId IS NULL OR LEN(TRIM(c.NationalId)) NOT IN ({_naturalNationalCodeCharecter},{_lagalNationalCodeCharecter}) THEN 1 ELSE null END ) InvalidCount
                    From CustomerWarehouse.dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.ZoneId IN @ZoneIds AND
                    	c.DeletionStateId IN (0,5)
                    Group By c.ZoneTitle";
        }
    }
}
