using Aban360.Common.Db.Dapper;
using Aban360.Common.Exceptions;
using Aban360.Common.Literals;
using Aban360.ReportPool.Domain.Constants;
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
        private int _legalNationalCodeCharecter = 11;
        private int _naturalNationalCodeCharecter = 10;
        private int _invalidNationalCodeCharecter = 0;
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
        public async Task<IEnumerable<CustomerLegalDetailDataOutputDto>> GetDetail(CustomerLegalDetailInputDto input)
        {
            string query = GetLegalsInfoDetailQuery(input.Type);
            IEnumerable<CustomerLegalDetailDataOutputDto> data = await _sqlReportConnection.QueryAsync<CustomerLegalDetailDataOutputDto>(query, input);
            return data;
        }
        public async Task<IEnumerable<CustomerLegalSummaryDataOutputDto>> GetSummary(CustomerLegalSummaryDto input)
        {
            var (groupId, groupTitle) = GetGroupField(input.IsZone);
            string query = GetLegalsInfoSummaryQuery(groupId, groupTitle);
            IEnumerable<CustomerLegalSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<CustomerLegalSummaryDataOutputDto>(query, input);
            return data;
        }
        public async Task<IEnumerable<CustomerLegalSummaryByZoneAndUsageDataOutputDto>> GetSummary(CustomerLegalSummaryByZoneAndUsageInputDto input)
        {
            string query = GetLegalsInfoSummaryByZoneAndUsageQuery();
            IEnumerable<CustomerLegalSummaryByZoneAndUsageDataOutputDto> data = await _sqlReportConnection.QueryAsync<CustomerLegalSummaryByZoneAndUsageDataOutputDto>(query, input);
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
        private string GetLegalsInfoDetailQuery(CustomerLegalDetailEnum type)
        {
            string nationalCondition = type switch
            {
                CustomerLegalDetailEnum.ValidLegal => $@" c.NationalId IS NOT NULL AND
						                                  LEN(c.NationalId) = {_legalNationalCodeCharecter} AND
						                                  c.NationalId NOT LIKE '%[^0-9]%' AND
						                                  c.NationalId NOT IN 
						                                  (
						                                      '00000000000',
						                                      '11111111111',
						                                      '22222222222',
						                                      '33333333333',
						                                      '44444444444',
						                                      '55555555555',
						                                      '66666666666',
						                                      '77777777777',
						                                      '88888888888',
						                                      '99999999999'
						                                  )AND
						                                  Legal.CheckDigit =
						                                      CASE
						                                          WHEN Legal.SumValue % 11 = 10
						                                              THEN 0
						                                          ELSE
						                                              Legal.SumValue % 11
						                                      END ",
                CustomerLegalDetailEnum.InValidLegal => $@" c.NationalId IS NOT NULL AND
						                                    LEN(c.NationalId) = {_legalNationalCodeCharecter} AND
						                                    (
						                                    	c.NationalId LIKE '%[^0-9]%' OR
						                                    	c.NationalId IN 
						                                    	(
						                                    	    '00000000000',
						                                    	    '11111111111',
						                                    	    '22222222222',
						                                    	    '33333333333',
						                                    	    '44444444444',
						                                    	    '55555555555',
						                                    	    '66666666666',
						                                    	    '77777777777',
						                                    	    '88888888888',
						                                    	    '99999999999'
						                                    	) OR
						                                    	Legal.CheckDigit <>
						                                    	    CASE
						                                    	        WHEN Legal.SumValue % 11 = 10
						                                    	            THEN 0
						                                    	        ELSE
						                                    	            Legal.SumValue % 11
						                                    	    END
						                                    ) ",
                CustomerLegalDetailEnum.ValidNatural => $@" c.NationalId IS NOT NULL AND
						                                    LEN(c.NationalId)={_naturalNationalCodeCharecter} AND
						                                    c.NationalId NOT LIKE '%[^0-9]%' AND
						                                    c.NationalId NOT IN
						                                    (
						                                        '0000000000',
						                                        '1111111111',
						                                        '2222222222',
						                                        '3333333333',
						                                        '4444444444',
						                                        '5555555555',
						                                        '6666666666',
						                                        '7777777777',
						                                        '8888888888',
						                                        '9999999999'
						                                    ) AND
						                                    Natural.CheckDigit =
						                                       CASE
						                                           WHEN Natural.SumValue % 11 < 2
						                                               THEN Natural.SumValue % 11
						                                           ELSE
						                                               11 - (Natural.SumValue % 11)
						                                       END   ",
                CustomerLegalDetailEnum.InValidNatural => $@" c.NationalId IS NOT NULL AND
						                                      LEN(c.NationalId)={_naturalNationalCodeCharecter} AND
						                                      (
						                                      	c.NationalId LIKE '%[^0-9]%' OR
						                                      	c.NationalId IN
						                                      	(
						                                      	    '0000000000',
						                                      	    '1111111111',
						                                      	    '2222222222',
						                                      	    '3333333333',
						                                      	    '4444444444',
						                                      	    '5555555555',
						                                      	    '6666666666',
						                                      	    '7777777777',
						                                      	    '8888888888',
						                                      	    '9999999999'
						                                      	) OR
						                                      	Natural.CheckDigit <>
						                                      	   CASE
						                                      	       WHEN Natural.SumValue % 11 < 2
						                                      	           THEN Natural.SumValue % 11
						                                      	       ELSE
						                                      	           11 - (Natural.SumValue % 11)
						                                      	   END          
						                                      )",
                CustomerLegalDetailEnum.Invalid => $@" c.NationalId IS NOT NULL AND 
                                                       (
                                                            LEN(c.NationalId)>{_invalidNationalCodeCharecter} AND 
                                                            LEN(c.NationalId)<{_naturalNationalCodeCharecter}
                                                       ) OR
                                                       LEN(c.NationalId)>{_legalNationalCodeCharecter} ",
                CustomerLegalDetailEnum.Empty => $@" c.NationalId IS NULL AND
                                                     LEN(c.NationalId)={_invalidNationalCodeCharecter} ",
                _ => $@" c.NationalId IS NOT NULL ",
            };
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
                    CROSS APPLY
					(
					    SELECT
					        CheckDigit = ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48,
					        SumValue =
					              (ASCII(SUBSTRING(c.NationalId, 1, 1)) - 48) * 10
					            + (ASCII(SUBSTRING(c.NationalId, 2, 1)) - 48) * 9
					            + (ASCII(SUBSTRING(c.NationalId, 3, 1)) - 48) * 8
					            + (ASCII(SUBSTRING(c.NationalId, 4, 1)) - 48) * 7
					            + (ASCII(SUBSTRING(c.NationalId, 5, 1)) - 48) * 6
					            + (ASCII(SUBSTRING(c.NationalId, 6, 1)) - 48) * 5
					            + (ASCII(SUBSTRING(c.NationalId, 7, 1)) - 48) * 4
					            + (ASCII(SUBSTRING(c.NationalId, 8, 1)) - 48) * 3
					            + (ASCII(SUBSTRING(c.NationalId, 9, 1)) - 48) * 2
					) Natural
					CROSS APPLY
					(
					    SELECT
					        CheckDigit = ASCII(SUBSTRING(c.NationalId, 11, 1)) - 48,
					        SumValue =
					              (
					                  (ASCII(SUBSTRING(c.NationalId, 1, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 29
					            + (
					                  (ASCII(SUBSTRING(c.NationalId, 2, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 27
					
					            + (
					                  (ASCII(SUBSTRING(c.NationalId, 3, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 23
					
					            + (
					                  (ASCII(SUBSTRING(c.NationalId, 4, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 19
					
					            + (
					                  (ASCII(SUBSTRING(c.NationalId, 5, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 17
					
					            + (
					                  (ASCII(SUBSTRING(c.NationalId, 6, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 29
					
					            + (
					                  (ASCII(SUBSTRING(c.NationalId, 7, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 27
					
					            + (
					                  (ASCII(SUBSTRING(c.NationalId, 8, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 23
					
					            + (
					                  (ASCII(SUBSTRING(c.NationalId, 9, 1)) - 48) + (ASCII(SUBSTRING(c.NationalId, 10, 1)) - 48) + 2
					              ) * 19
					) Legal
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.ZoneId IN @ZoneIds AND
                    	{nationalCondition}";
        }
        private string GetLegalsInfoSummaryQuery(string groupId, string groupTitle)
        {
            return @$"Select 
                    	MAX({groupId}) ItemId,
                    	{groupTitle} ItemTitle,
                    	COUNT(CASE WHEN c.NationalId IS NOT NULL AND LEN(c.NationalId)={_naturalNationalCodeCharecter} THEN 1 ELSE null END ) NaturalCount,
                    	COUNT(CASE WHEN c.NationalId IS NOT NULL AND LEN(c.NationalId)={_legalNationalCodeCharecter} THEN 1 ELSE null END ) LegalCount,
                    	COUNT(CASE WHEN c.NationalId IS NULL OR LEN(c.NationalId) NOT IN ({_naturalNationalCodeCharecter},{_legalNationalCodeCharecter}) THEN 1 ELSE null END ) InvalidCount
                    From CustomerWarehouse.dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	{groupId} IN @ItemIds AND
                    	c.DeletionStateId IN (0,5)
                    Group By {groupTitle}";
        }
        private string GetLegalsInfoSummaryByZoneAndUsageQuery()
        {
            return @$"Select 
                    	MAX(c.ZoneId) ZoneId,
                    	c.ZoneTitle ZoneTitle,
                        MAX(c.UsageId) UsageId,
                    	c.UsageTitle UsageTitle,
                    	COUNT(CASE WHEN c.NationalId IS NOT NULL AND LEN(c.NationalId)={_naturalNationalCodeCharecter} THEN 1 ELSE null END ) NaturalCount,
                    	COUNT(CASE WHEN c.NationalId IS NOT NULL AND LEN(c.NationalId)={_legalNationalCodeCharecter} THEN 1 ELSE null END ) LegalCount,
                    	COUNT(CASE WHEN c.NationalId IS NULL OR LEN(c.NationalId) NOT IN ({_naturalNationalCodeCharecter},{_legalNationalCodeCharecter}) THEN 1 ELSE null END ) InvalidCount
                    From CustomerWarehouse.dbo.Clients c
                    Where 
                    	c.ToDayJalali IS NULL AND
                    	c.ZoneId IN @ZoneIds AND
                        c.UsageId IN @UsageIds AND
                    	c.DeletionStateId IN (0,5)
                    Group By c.ZoneTitle,c.UsageTitle
                    Order By c.ZoneTitle,c.UsageTitle";
        }
        private (string, string) GetGroupField(bool isZone)
        {
            return isZone ? (" c.ZoneId ", " c.ZoneTitle ") : (" c.UsageId ", " c.UsageTitle ");
        }
    }
}
