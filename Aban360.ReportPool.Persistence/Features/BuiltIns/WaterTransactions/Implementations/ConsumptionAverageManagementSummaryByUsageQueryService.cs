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
        public async Task<IEnumerable<ConsumptionAverageManagementSummaryOutputDto>> Get(ConsumptionAverageManagementSummrayInputDto input)
        {
            string query = GetManagementQuery();
            IEnumerable<ConsumptionAverageManagementSummaryOutputDto> result = await _sqlReportConnection.QueryAsync<ConsumptionAverageManagementSummaryOutputDto>(query, input);
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
        private string GetManagementQuery()
        {
            return @"Select 
                		b.BillId,
                		MAX(b.ZoneId) ZoneId, 
                		MAX(b.ZoneTitle) ZoneTitle, 
                		MAX(c.UsageId) UsageId,
                		MAX(c.UsageTitle) UsageTitle,
                		AVG(b.Consumption) Consumption,
                		AVG(b.ConsumptionAverage)ConsumptionAverage,
                		CASE WHEN @IsDomestic=1 THEN MAX(s.olgo) ELSE MAX(b.ContractCapacity) END as ContracutalOrOlgo,
                		'' RegisterDateJalali
                	From CustomerWarehouse.dbo.Bills b
                	Join CustomerWarehouse.dbo.Clients c
                		ON b.ZoneId=c.ZoneId AND b.CustomerNumber=c.CustomerNumber
                	Outer apply(
                		Select top 1 *
                		from OldCalc.dbo.S s
                		where  b.ZoneId=s.town AND b.RegisterDay COLLATE Persian_100_CI_AI  BETWEEN s.FromDate AND s.ToDate
                		order by s.ToDate Desc
                		)s
                	Where 
                		c.ToDayJalali IS NULL AND
                		( (@IsDomestic=1 AND c.UsageId IN (0,1,3) AND c.UsageStateId<>4) OR (@IsDomestic<>1 AND c.UsageId NOT IN (0,1,3)) ) AND
                		( (@IsNet=1 AND b.TypeCode IN (1,3,4,5)) OR (@IsNet<>1 AND b.TypeCode IN (1)) ) AND
                		b.ZoneId IN @zoneIds AND
                		b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali
                	Group By b.BillId";
            //return @"Select 
            //        	b.ZoneId, 
            //        	b.ZoneTitle,
            //        	b.UsageId,
            //        	b.UsageTitle,
            //        	b.Consumption,
            //        	b.ConsumptionAverage,
            //        	CASE WHEN @IsDomestic=1 THEN s.olgo ELSE b.ContractCapacity END as ContracutalOrOlgo,
            //        	b.RegisterDay as RegisterDateJalali
            //        From CustomerWarehouse.dbo.Bills b
            //        Left Join OldCalc.dbo.S s
            //        	on b.ZoneId=s.town AND b.RegisterDay COLLATE Persian_100_CI_AI  BETWEEN s.FromDate AND s.ToDate
            //        Where 
            //        	( (@IsDomestic=1 AND b.UsageId IN (0,1,3)) OR (@IsDomestic<>1 AND b.UsageId NOT IN (0,1,3)) ) AND
            //        	( (@IsNet=1 AND b.TypeCode IN (1,3,4,5)) OR (@IsNet<>1 AND b.TypeCode IN (1)) ) AND
            //        	b.ZoneId IN @ZoneIds AND
            //        	b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali";
        }
    }
}
