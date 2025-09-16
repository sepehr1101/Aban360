using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class RuinedMeterIncomeSummaryByUsageQueryService : AbstractBaseConnection, IRuinedMeterIncomeSummaryByUsageQueryService
    {
        public RuinedMeterIncomeSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>> GetInfo(RuinedMeterIncomeInputDto input)
        {
            string ruinedMeterIncomeQueryString = GetRuinedMeterIncomeDataQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<RuinedMeterIncomeSummaryDataOutputDto> ruinedMeterIncomeData = await _sqlReportConnection.QueryAsync<RuinedMeterIncomeSummaryDataOutputDto>(ruinedMeterIncomeQueryString, @params);
            RuinedMeterIncomeHeaderOutputDto ruinedMeterIncomeHeader = new RuinedMeterIncomeHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = ruinedMeterIncomeData is not null && ruinedMeterIncomeData.Any() ? ruinedMeterIncomeData.Count() : 0,

                TotalPayable = ruinedMeterIncomeData.Sum(x => x.Payable),
                TotalSumItems = ruinedMeterIncomeData.Sum(x => x.SumItems),
                SumCommercialUnit = ruinedMeterIncomeData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = ruinedMeterIncomeData.Sum(i => i.DomesticUnit),
                SumOtherUnit = ruinedMeterIncomeData.Sum(i => i.OtherUnit),
                TotalUnit = ruinedMeterIncomeData.Sum(i => i.TotalUnit),
                CustomerCount = ruinedMeterIncomeData.Sum(i => i.CustomerCount),
            };
            var result = new ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>(ReportLiterals.RuinedMeterIncomeSummary + ReportLiterals.ByUsage + ReportLiterals.ByZone, ruinedMeterIncomeHeader, ruinedMeterIncomeData);

            return result;
        }

        private string GetRuinedMeterIncomeDataQuery()
        {
            return @";WITH MalfunctionIncome as(
                    Select 
                    	 b.UsageTitle,
                         b.UsageId,
						 b.WaterDiameterId,
						 b.SumItems,
						 b.Payable,
						 b.CommercialCount,
						 b.DomesticCount,
						 b.OtherCount,
						 b.Consumption,
						 b.Duration	
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.ZoneId IN @zoneIds AND
                    	b.CounterStateCode=1)
                    Select	
						m.UsageTitle as ItemTitle,		
						COUNT(m.UsageTitle) AS CustomerCount,
						SUM(m.Duration) AS SumDuration,
						AVG(m.Duration) AS AverageDuration,
						SUM(m.Consumption) AS SumConsumption,
						AVG(m.Consumption) AS AverageConsumption,
					    SUM(m.Payable) AS Payable,
					    SUM(m.SumItems) AS SumItems,
					    SUM(ISNULL(m.CommercialCount, 0) + ISNULL(m.DomesticCount, 0) + ISNULL(m.OtherCount, 0)) AS TotalCount,
					    SUM(ISNULL(m.CommercialCount, 0)) AS CommercialCount,
                        SUM(ISNULL(m.DomesticCount, 0)) AS DomesticCount,
                        SUM(ISNULL(m.OtherCount, 0)) AS OtherCount,
						SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
						SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
						SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
						SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
						SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
						SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
						SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
						SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
						SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
						SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From MalfunctionIncome m
                    Join [Db70].dbo.T5 t5
                    	On t5.C0=m.WaterDiameterId
                    Group By m.UsageTitle";
        }

    }
}
