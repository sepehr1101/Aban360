using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
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
    internal sealed class ContractualCapacityQueryService : AbstractBaseConnection, IContractualCapacityQueryService
    {
        public ContractualCapacityQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>> GetInfo(ContractualCapacityInputDto input)
        {
            string query = GetQuery(input.UsageSellIds.HasValue(), input.ZoneIds.HasValue());
            var @params = new
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                
                FromCapacity = input.FromContractualCapacity != null ? input.FromContractualCapacity : 1,
                ToCapacity = input.ToContractualCapacity != null ? input.ToContractualCapacity : 999999,

                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,

                UsageIds = input.UsageSellIds,
                ZoneIds = input.ZoneIds
            };

            IEnumerable<ContractualCapacityDataOutputDto> contractualCapacityData = await _sqlReportConnection.QueryAsync<ContractualCapacityDataOutputDto>(query, @params);
            ContractualCapacityHeaderOutputDto contractualCapacityHeader = new ContractualCapacityHeaderOutputDto()
            {
                FromContractualCapacity = input.FromContractualCapacity,
                ToContractualCapacity = input.ToContractualCapacity,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali=input.FromDateJalali,
                ToDateJalali=input.ToDateJalali,
                RecordCount = (contractualCapacityData is not null && contractualCapacityData.Any()) ? contractualCapacityData.Count() : 0,
                CustomerCount = (contractualCapacityData is not null && contractualCapacityData.Any()) ? contractualCapacityData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title= ReportLiterals.ContractualCapacity,

                SumCommercialUnit=contractualCapacityData?.Sum(t=>t.CommercialUnit)??0,
                SumDomesticUnit=contractualCapacityData?.Sum(t=>t.DomesticUnit)??0,
                SumOtherUnit=contractualCapacityData?.Sum(t=>t.OtherUnit)??0,
                TotalUnit=contractualCapacityData?.Sum(t=>t.TotalUnit)??0,
            };

            var result = new ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>(ReportLiterals.ContractualCapacity, contractualCapacityHeader, contractualCapacityData);
            return result;
        }

        private string GetQuery(bool hasUsage, bool hasZone)
        {
            string usageQuery = hasUsage ? "c.UsageId in @UsageIds AND" : string.Empty;
            string zoneQuery = hasZone ? " c.ZoneId in @ZoneIds AND" : string.Empty;

            return @$";WITH CTE AS
                    (
                    	SELECT 
                    	    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                    	    *
                    	From [CustomerWarehouse].dbo.Clients c
                    	Where				
                    	    c.WaterInstallDate BETWEEN @FromDate AND @ToDate AND
                    	    {zoneQuery}                   	    
                    	    (
                    	        @fromReadingNumber IS NULL OR 
                    	        @toReadingNumber IS NULL OR
                    	        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                    	    ) AND
                    	    c.CustomerNumber<>0 
                    )
                    Select	
                    	 c.CustomerNumber,
                        c.ReadingNumber,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) As Surname,
                        c.UsageTitle,
                        c.WaterDiameterTitle MeterDiameterTitle,
                        c.RegisterDayJalali AS EventDateJalali,
                        TRIM(c.Address) AS Address,
                        c.ZoneTitle,
                        c.DeletionStateId,
                        c.DeletionStateTitle AS UseStateTitle,
                        c.DomesticCount DomesticUnit,
            	        c.CommercialCount CommercialUnit,
            	        c.OtherCount OtherUnit,
                        IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
            	        TRIM(c.BillId) BillId,
            			c.ContractCapacity As ContractualCapacity
                     FROM CTE c
                     WHERE	  
                         c.RN=1 AND
                         c.DeletionStateId NOT IN(1,2) AND
						 {usageQuery}
						 c.ContractCapacity BETWEEN @FromCapacity AND @ToCapacity";
        }
    }
}
