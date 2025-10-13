using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
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
            string customerSearchDataInfoQuery = GetCustomerSearchDataQuery(input.UsageIds?.Any()==true, input.ZoneIds?.Any()==true);
            var @params = new
            {
                input.CustomerNumber,
                input.FromReadingNumber,
                input.ToReadingNumber,
                input.FirstName,
                SureName = input.Surname,
                WaterDiameterId = input.MeterDiameter,
                input.BillId,
                FromDomesticCount = input.FromUnitDomesticWater,
                ToDomesticCount = input.ToUnitDomesticWater,
                FromCommercialCount = input.FromUnitCommercialWater,
                ToCommercialCount = input.ToUnitCommercialWater,
                FromOtherCount = input.FromUnitOtherWater,
                ToOtherCount = input.ToUnitOtherWater,
                MobileNo = input.MobileNumber,
                input.Address,
                input.ZoneIds,
                input.FromContractualCapacity,
                input.ToContractualCapacity,
                input.FromHousholderNumber,
                input.ToHousholderNumber,
                input.UsageIds,
                nationalCode = input.NationalCode,
                postalCode = input.PostalCode,
                phoneNumber = input.PhoneNumber,
            };

            IEnumerable<CustomerSearchDataOutputDto> customerData = await _sqlReportConnection.QueryAsync<CustomerSearchDataOutputDto>(customerSearchDataInfoQuery, @params, null, 120);//todo: send parameters
            CustomerSearchHeaderOutputDto customerHeader = new CustomerSearchHeaderOutputDto()
            {
                CustomerCount = (customerData is not null && customerData.Any()) ? customerData.Count() : 0,
                RecordCount = (customerData is not null && customerData.Any()) ? customerData.Count() : 0,
                ReportDateJalali =DateTime.Now.ToShortPersianDateString(),
                Title = ReportLiterals.CustomerSearch
            };

            var result = new ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>(ReportLiterals.CustomerSearch, customerHeader, customerData);
            return result;
        }

        private string GetCustomerSearchDataQuery(bool hasUsageId, bool hasZoneIds)
        {
            string usageIdsPartialQuery = hasUsageId ? "AND c.UsageId IN @UsageIds" : string.Empty;
            string zoneIdsPartialQuery = hasZoneIds ? "AND c.ZoneId IN @ZoneIds" : string.Empty;

            return @$"SELECT TOP(1000)
                        c.CustomerNumber,
                        c.ReadingNumber,
                        TRIM(c.FirstName) AS FirstName,
                      	TRIM(c.SureName) AS Surname,
                        c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.BillId,
                        c.DomesticCount AS DomesticUnit,
                        c.CommercialCount AS CommercialUnit,
                        c.OtherCount AS OtherUnit,
                        c.MobileNo AS MobileNumber,
                        TRIM(c.Address) AS Address,
                        c.HasCommonSiphon AS CommonSiphon,
						c.IsSpecial AS SpecialCustomer,
						c.PhoneNo AS PhoneNumber,
						c.NationalId AS NationalCode,
						c.PostalCode AS PostalCode
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
                        c.ToDayJalali is null
                        --AND (@ReadingNumber is null OR c.ReadingNumber like '%'+@ReadingNumber+'%')
                        AND (@FirstName is null OR c.FirstName like '%'+@FirstName+'%')
                        AND (@SureName is null OR c.SureName like '%'+@SureName+'%')
                        AND (@BillId is null OR c.BillId like '%'+@BillId+'%')
                        AND (@MobileNo is null OR c.MobileNo like '%'+@MobileNo+'%')
                        AND (@Address is null OR c.Address like '%'+@Address+'%')
                        AND c.CustomerNumber=IIF(@CustomerNumber IS NULL , c.CustomerNumber,@CustomerNumber) 
                        AND c.WaterDiameterId=IIF(@WaterDiameterId IS NULL, c.WaterDiameterId,@WaterDiameterId )
						AND c.PhoneNo=IIF(@phoneNumber IS NULL,c.PhoneNo,@phoneNumber) 
						AND c.PostalCode=IIF(@postalCode IS NULL,c.PostalCode,@postalCode)
						AND c.NationalId=IIF(@nationalCode IS NULL,c.NationalId,@nationalCode)
                        {zoneIdsPartialQuery}
                        AND (@FromReadingNumber IS NULL 
                             OR @ToReadingNumber IS NULL 
                             OR c.ReadingNumber between @FromReadingNumber AND @ToReadingNumber)
                        AND (@FromDomesticCount IS NULL 
                             OR @ToDomesticCount IS NULL 
                             OR c.DomesticCount between @FromDomesticCount AND @ToDomesticCount)
                        AND (@FromCommercialCount IS NULL 
                             OR @ToCommercialCount IS NULL
                             OR c.CommercialCount BETWEEN @FromCommercialCount AND @ToCommercialCount)
                        AND (@FromOtherCount IS NULL  
                             OR @ToOtherCount IS NULL
                             OR c.OtherCount BETWEEN @FromOtherCount AND @ToOtherCount)
					    AND (@FromContractualCapacity IS NULL  
                             OR @ToContractualCapacity IS NULL
                             OR c.ContractCapacity BETWEEN @FromContractualCapacity AND @ToContractualCapacity)
					    AND (@FromHousholderNumber IS NULL  
                             OR @ToHousholderNumber IS NULL
                             OR c.FamilyCount BETWEEN @FromHousholderNumber AND @ToHousholderNumber)
						{usageIdsPartialQuery}";
        }
    }
}