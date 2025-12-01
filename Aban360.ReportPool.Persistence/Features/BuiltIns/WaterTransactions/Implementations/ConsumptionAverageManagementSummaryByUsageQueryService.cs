using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ConsumptionAverageManagementSummaryByUsageQueryService : AbstractBaseConnection, IConsumptionAverageManagementSummaryByUsageQueryService
    {
        public ConsumptionAverageManagementSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryDataOutputDto>> Get(ConsumptionAverageManagementInputDto input, string groupField)
        {
            string reportTitle = string.Concat(ReportLiterals.ConsumptionManagerSummary + ReportLiterals.ByUsage);
            string query = GetQuery(groupField);
            IEnumerable<ConsumptionAverageManagementSummaryDataOutputDto> data = await _sqlReportConnection.QueryAsync<ConsumptionAverageManagementSummaryDataOutputDto>(query, input);
            ConsumptionAverageManagementHeaderOutputDto header = new ConsumptionAverageManagementHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                CustomerCount = data.Count(),
                RecordCount = data.Count(),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,

                Consumption = data.Count() > 0 ? data.Average(c => c.Consumption) : 0,
                ConsumptionAverage = data.Count() > 0 ? data.Average(c => c.ConsumptionAverage) : 0,
            };
            ReportOutput<ConsumptionAverageManagementHeaderOutputDto, ConsumptionAverageManagementSummaryDataOutputDto> result = new(reportTitle, header, data);

            return result;
        }
        private string GetQuery(string groupItem)
        {
            return @$"Select
                    	MAX(t46.C2) AS RegionTitle,
                    	b.{groupItem},
                    	b.{groupItem} as ItemTitle,
                    	COUNT(b.{groupItem}) as BillCount,
                    	AVG(b.Consumption) as Consumption,
                    	AVG(b.ConsumptionAverage) as ConsumptionAverage,
                    	SUM(IIF((b.DomesticCount+b.CommercialCount +b.OtherCount=0) ,1, (b.DomesticCount+b.CommercialCount +b.OtherCount))) AS TotalUnit,
                    	SUM(ISNULL(b.CommercialCount, 0)) AS CommercialUnit,
                    	SUM(ISNULL(b.DomesticCount, 0)) AS DomesticUnit,
                    	SUM(ISNULL(b.OtherCount, 0)) AS OtherUnit
                    From CustomerWarehouse.dbo.Bills b
                    Join [Db70].dbo.T51 t51
                    	On t51.C0=b.ZoneId
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
                    Where
                    	b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	b.ZoneId IN @zoneIds AND
                    	b.UsageId IN @usageIds
                    Group By b.{groupItem}";
        }
    }
}
