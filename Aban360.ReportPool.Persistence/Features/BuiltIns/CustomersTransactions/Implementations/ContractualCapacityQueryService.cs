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
        { }
        public async Task<ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>> GetInfo(ContractualCapacityInputDto input)
        {
            string contractualCapacityQuery = GetContractualCapacityQuery();
            var @params = new
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,

                FromCapacity = input.FromContractualCapacity,
                ToCapacity = input.ToContractualCapacity,

                UsageIds = input.UsageSellIds,
                ZoneIds = input.ZoneIds
            };

            IEnumerable<ContractualCapacityDataOutputDto> contractualCapacityData = await _sqlReportConnection.QueryAsync<ContractualCapacityDataOutputDto>(contractualCapacityQuery,@params);
            ContractualCapacityHeaderOutputDto contractualCapacityHeader = new ContractualCapacityHeaderOutputDto()
            { 
                RecordCount=contractualCapacityData.Count(),
                ReportDate=DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<ContractualCapacityHeaderOutputDto, ContractualCapacityDataOutputDto>(ReportLiterals.ContractualCapacity, contractualCapacityHeader, contractualCapacityData);

            return result;
        }

        private string GetContractualCapacityQuery()
        {
            return @"SELECT 
                        c.CustomerNumber,
                        c.ReadingNumber,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) As Surname,
                        c.UsageTitle,
                        c.WaterDiameterTitle MeterDiameterTitle,
                        c.RegisterDayJalali AS EventDateJalali,
                        0 AS DebtAmount,
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
            			c.ToDayJalali IS NULL AND
            			c.UsageId in @UsageIds AND
                        c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber AND
                        c.ZoneId in @ZoneIds AND
            			c.ContractCapacity BETWEEN @FromCapacity AND @ToCapacity";
        }
    }
}
