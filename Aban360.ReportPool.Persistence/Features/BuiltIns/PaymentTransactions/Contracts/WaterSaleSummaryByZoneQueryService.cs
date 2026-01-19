using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts
{
    internal sealed class WaterSaleSummaryByZoneQueryService : AbstractBaseConnection, IWaterSaleSummaryByZoneQueryService
    {
        public WaterSaleSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterSaleSummaryByZoneHeaderOutputDto, WaterSaleSummaryByZoneDataOutputDto>> Get(WaterSaleSummaryByZoneInputDto input)
        {
            string reportTitle = ReportLiterals.WaterSaleSummary;
            string query = GetQuery(input.HasUsageGroup, input.IsNet);

            IEnumerable<WaterSaleSummaryByZoneDataOutputDto> data = await _sqlReportConnection.QueryAsync<WaterSaleSummaryByZoneDataOutputDto>(query, input);
            WaterSaleSummaryByZoneHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                IsNet = input.IsNet,
                BillCount = data.Sum(x => x.BillCount),
                CustomerCount = data.Sum(x => x.CustomerCount),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,
            };

            ReportOutput<WaterSaleSummaryByZoneHeaderOutputDto, WaterSaleSummaryByZoneDataOutputDto> result = new(reportTitle, header, data);
			return result;
        }
        private string GetQuery(bool hasUsageGroup, bool isNet)
        {
            string usageGroup = hasUsageGroup ? " b.UsageTitle, " : string.Empty;
            string typeCodeCondition = isNet ? "1,3,4,5" : "1";

            return $@";With Cte as(
						Select *
						From CustomerWarehouse.dbo.Bills b
						Where		
							b.RegisterDay BETWEEN @FromDateJalali AND ToDateJalali AND 
							b.ZoneId IN @ZoneIds AND
							b.UsageId IN @UsageIds AND
							b.BranchTypeId IN @BranchTypeIds AND	
							b.TypeCode IN ({typeCodeCondition}) 
					)
					Select	
						b.ZoneTitle,
						{usageGroup}
						AVG(Case
								WHEN LEN(TRIM(b.PreviousDay))=10 AND LEN(TRIM(b.NextDay))=10 
								THEN DATEDIFF(Day,CustomerWarehouse.dbo.PersianToMiladi(b.PreviousDay),CustomerWarehouse.dbo.PersianToMiladi(b.NextDay)) 
								Else null
							End) DifferentDay,
						Case WHEN FLOOR(b.ConsumptionAverage)>60 THEN 99 Else Floor(b.ConsumptionAverage) End ConsumptionAverage, 
						COUNT(b.ConsumptionAverage) BillCount,
						COUNT(Distinct b.BillId) CustomerCount,
						SUM(b.DomesticCount) DomesticUnit,
						SUM(b.CommercialCount) CommertialUnit,
						SUM(b.OtherCount) OtherUnit,
						SUM(IIF((b.DomesticCount+b.CommercialCount +b.OtherCount=0) ,1, (b.DomesticCount+b.CommercialCount +b.OtherCount))) AS TotalUnit,
						SUM(Case	
							When LEN(TRIM(c.PhysicalSewageInstallDateJalali))=10 
							THEN IIF((b.DomesticCount+b.CommercialCount +b.OtherCount=0) ,1, (b.DomesticCount+b.CommercialCount +b.OtherCount))
							Else 0
						END) SewageUnitCount,
						AVG(b.Duration) DurationAverage,--Or these 
						SUM(b.Duration) DurationSum,--these
						SUM(b.Item1) AbBaha,
						SUM(b.Item2) BazelabBaha,
						SUM(b.Item3) AbonAb,
						SUM(b.Item4) AbonFazelab,
						SUM(b.Item5) Shahrdari,
						SUM(b.Item6) Tabsare2,
						SUM(b.Item8) Jarime,
						SUM(b.Item9) Abresani,
						SUM(b.Item10) JavaniD,
						SUM(b.Item11) HotSeason,
						SUM(b.Item12) ZaribTadil,
						SUM(b.Item18) Boodje,
						SUM(b.Item17) Lavazem
					From Cte b
					Left Join CustomerWarehouse.dbo.Clients c
						ON b.ZoneId=c.ZoneId AND b.CustomerNumber=c.CustomerNumber
					Where
						c.ToDayJalali IS NULL AND
						c.DeletionStateId NOT IN (1,2)
					Group By b.ZoneTitle,{usageGroup} FLOOR(b.ConsumptionAverage) 
					Order By b.ZoneTitle,{usageGroup} FLOOR(b.ConsumptionAverage)";
        }
    }
}
