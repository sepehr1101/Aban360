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
    internal sealed class RuinedMeterIncomeDetailQueryService : AbstractBaseConnection, IRuinedMeterIncomeDetailQueryService
    {
        public RuinedMeterIncomeDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeDetailDataOutputDto>> GetInfo(RuinedMeterIncomeInputDto input)
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
            IEnumerable<RuinedMeterIncomeDetailDataOutputDto> ruinedMeterIncomeData = await _sqlReportConnection.QueryAsync<RuinedMeterIncomeDetailDataOutputDto>(ruinedMeterIncomeQueryString, @params);
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

            var result = new ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeDetailDataOutputDto>(ReportLiterals.RuinedMeterIncomeDetail, ruinedMeterIncomeHeader, ruinedMeterIncomeData);

            return result;
        }

        private string GetRuinedMeterIncomeDataQuery()
        {
            return @"Select 
	                    b.BillId,
                        b.ZoneTitle,
                        b.ZoneId,
                        b.CustomerNumber,
                        b.ReadingNumber,
                        b.Duration,
                        b.BranchType,
                        b.RegisterDay AS LastReadingDay,
                        b.Payable,
	                    b.SumItems,
                        b.Consumption,
                        b.ConsumptionAverage,
	                    b.CounterStateCode,
	                    b.CounterStateTitle,
						b.WaterDiameterTitle,
						b.UsageTitle
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.ZoneId IN @zoneIds AND
                    	b.CounterStateCode=1";
        }
    }
}
