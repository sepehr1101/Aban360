using Aban360.Common.Db.Dapper;
using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class RuinedMeterIncomeSummaryQueryService : AbstractBaseConnection, IRuinedMeterIncomeSummaryQueryService
    {
        public RuinedMeterIncomeSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>> GetInfo(RuinedMeterIncomeInputDto input)
        {
            string ruinedMeterIncomeQueryString = GetRuinedMeterIncomeDataQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber=input.FromReadingNumber,
                toReadingNumber=input.ToReadingNumber,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<RuinedMeterIncomeSummaryDataOutputDto> ruinedMeterIncomeData = await _sqlReportConnection.QueryAsync<RuinedMeterIncomeSummaryDataOutputDto>(ruinedMeterIncomeQueryString, @params);
            RuinedMeterIncomeHeaderOutputDto ruinedMeterIncomeHeader = new RuinedMeterIncomeHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = ruinedMeterIncomeData is not null && ruinedMeterIncomeData.Any() ? ruinedMeterIncomeData.Count() : 0,
            };
            if (ruinedMeterIncomeData is not null && ruinedMeterIncomeData.Any())
            {
                ruinedMeterIncomeHeader.TotalPayable = ruinedMeterIncomeData.Sum(x => x.Payable);
                ruinedMeterIncomeHeader.TotalSumItems = ruinedMeterIncomeData.Sum(x => x.SumItems);
            }

            var result = new ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>(ReportLiterals.RuinedMeterIncome, ruinedMeterIncomeHeader, ruinedMeterIncomeData);

            return result;
        }

        private string GetRuinedMeterIncomeDataQuery()
        {
            return @"Select 
                    	 b.ZoneId,
                    	 b.ZoneTitle,
                    	 COUNT(1) AS RuinedCount,
                    	 SUM(b.Payable) AS Payable,
                    	 SUM(b.SumItems) AS SumItems
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.ZoneId IN @zoneIds AND
                    	b.CounterStateCode=1
                    Group By 
                    	b.ZoneId,
                    	b.ZoneTitle";
        }

    }
}
