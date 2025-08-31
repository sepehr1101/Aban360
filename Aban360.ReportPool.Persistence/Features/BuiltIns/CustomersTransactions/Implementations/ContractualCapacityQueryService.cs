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
    internal sealed class ContractualCapacityQueryService : AbstractBaseConnection, IContractualCapacityQueryService
    {
        public ContractualCapacityQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>> GetInfo(ContractualCapacityInputDto input)
        {
            string contractualCapacityQuery = GetContractualCapacityQuery(input.UsageSellIds?.Any()==true, input.ZoneIds?.Any() == true);
            var @params = new
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                
                //todo:When FromContracualCapacity is null Then 0 OR 1 ?
                FromCapacity = input.FromContractualCapacity != null ? input.FromContractualCapacity : 1,
                ToCapacity = input.ToContractualCapacity != null ? input.ToContractualCapacity : 999999,

                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,

                UsageIds = input.UsageSellIds,
                ZoneIds = input.ZoneIds
            };

            IEnumerable<ContractualCapacityDataOutputDto> contractualCapacityData = await _sqlReportConnection.QueryAsync<ContractualCapacityDataOutputDto>(contractualCapacityQuery, @params);
            ContractualCapacityHeaderOutputDto contractualCapacityHeader = new ContractualCapacityHeaderOutputDto()
            {
                FromContractualCapacity = input.FromContractualCapacity,
                ToContractualCapacity = input.ToContractualCapacity,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = (contractualCapacityData is not null && contractualCapacityData.Any()) ? contractualCapacityData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>(ReportLiterals.ContractualCapacity, contractualCapacityHeader, contractualCapacityData);
            return result;
        }

        private string GetContractualCapacityQuery(bool hasUsage, bool hasZone)
        {
            string usageQuery = hasUsage ? "c.UsageId in @UsageIds AND" : string.Empty;
            string zoneQuery = hasZone ? " c.ZoneId in @ZoneIds AND" : string.Empty;

            return @$"SELECT 
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
            	        TRIM(c.BillId) BillId,
            			c.ContractCapacity As ContractualCapacity
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
                        {usageQuery}
                        {zoneQuery}
            			c.ToDayJalali IS NULL AND
                        --c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber AND
                        (@FromReadingNumber is null Or
						@ToReadingNumber is null Or
						c.ReadingNumber between @FromReadingNumber and @ToReadingNumber) AND
            			c.ContractCapacity BETWEEN @FromCapacity AND @ToCapacity AND
                        c.WaterInstallDate BETWEEN @FromDate AND @ToDate";
        }
    }
}
