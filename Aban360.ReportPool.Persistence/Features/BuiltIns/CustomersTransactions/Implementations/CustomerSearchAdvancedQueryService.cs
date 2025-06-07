using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;

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
            string customerSearchDataInfoQuery = GetCustomerSearchDataQuery(input.UsageIds?.Any()==true);
            var @params = new
            {
                CustomerNumber = input.CustomerNumber,
                ReadingNumber = input.ReadingNumber,
                FirstName = input.FirstName,
                SureName = input.Surname,
                WaterDiameterId = input.MeterDiameter,
                BillId = input.BillId,
                FromDomesticCount = input.FromUnitDomesticWater,
                ToDomesticCount = input.ToUnitDomesticWater,
                FromCommercialCount = input.FromUnitCommercialWater,
                ToCommercialCount = input.ToUnitCommercialWater,
                FromOtherCount = input.FromUnitOtherWater,
                ToOtherCount = input.ToUnitOtherWater,
                MobileNo = input.MobileNumber,
                Address = input.Address,
                ZoneId = input.ZoneId,
                FromContractualCapacity = input.FromContractualCapacity,
                ToContractualCapacity = input.ToContractualCapacity,
                FromHousholderNumber = input.FromHousholderNumber,
                ToHousholderNumber = input.ToHousholderNumber,
                UsageIds= input.UsageIds
            };

            IEnumerable<CustomerSearchDataOutputDto> customerData = await _sqlConnection.QueryAsync<CustomerSearchDataOutputDto>(customerSearchDataInfoQuery, @params);//todo: send parameters
            CustomerSearchHeaderOutputDto customerHeader = new CustomerSearchHeaderOutputDto()
            {
                RecordCount = customerData.Count()
            };

            var result = new ReportOutput<CustomerSearchHeaderOutputDto, CustomerSearchDataOutputDto>(ReportLiterals.CustomerSearch, customerHeader, customerData);
            return result;
        }

        private string GetCustomerSearchDataQuery(bool HasUsageId)
        {
            string usageIds = HasUsageId ? "AND c.UsageId IN @UsageIds" : string.Empty;

            return @$"SELECT TOP(100)
                        c.CustomerNumber,
                        c.ReadingNumber,
                        c.FirstName,
                        c.SureName AS Surname,
                        c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.BillId,
                        c.DomesticCount AS UnitDomesticWater,
                        c.CommercialCount AS UnitCommercialWater,
                        c.OtherCount AS UnitOtherWater,
                        c.MobileNo AS MobileNumber,
                        c.Address
                    FROM Client1000 c
                    WHERE 
                            c.ToDayJalali is null
                        AND (@ReadingNumber is null OR c.ReadingNumber like '%'+@ReadingNumber+'%')
                        AND (@FirstName is null OR c.FirstName like '%'+@FirstName+'%')
                        AND (@SureName is null OR c.SureName like '%'+@SureName+'%')
                        AND (@BillId is null OR c.BillId like '%'+@BillId+'%')
                        AND (@MobileNo is null OR c.MobileNo like '%'+@MobileNo+'%')
                        AND (@Address is null OR c.Address like '%'+@Address+'%')
                        AND c.CustomerNumber=IIF(@CustomerNumber IS NULL , c.CustomerNumber,@CustomerNumber) 
                        AND c.WaterDiameterId=IIF(@WaterDiameterId IS NULL , c.WaterDiameterId,@WaterDiameterId )
                        AND c.ZoneId=IIF(@ZoneId IS NULL , c.ZoneId,@ZoneId)
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
						{usageIds}";
        }
    }
}
