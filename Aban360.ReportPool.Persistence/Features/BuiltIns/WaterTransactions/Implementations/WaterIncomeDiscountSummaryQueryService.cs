using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterIncomeDiscountSummaryQueryService : WaterIncomeDiscountBase, IWaterIncomeDiscountSummaryQueryService
    {
        public WaterIncomeDiscountSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterIncomeDiscountSummaryHeaderOutputDto, WaterIncomeDiscountSummaryDataOutputDto>> Get(WaterIncomeDiscountSummaryInputDto input)
        {
            string reportTitle = ReportLiterals.WaterIncomeDiscountSummary + GetIsZoneOrVillageTitle(input.ZoneIds);
            string waterIncomeDiscountSummarys = GetSummaryQuery(false, input.ZoneIds.HasValue(), input.DiscountCauseId, input.SummaryType);

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                fromConsumption = input.FromConsumption,
                toConsumption = input.ToConsumption,

                fromAmount = input.FromAmount,
                toAmount = input.ToAmount,

                typeCodes = GetTypeCodes(input.type),

                zoneIds = input.ZoneIds,
            };
            IEnumerable<WaterIncomeDiscountSummaryDataOutputDto> waterIncomeDiscountData = await _sqlReportConnection.QueryAsync<WaterIncomeDiscountSummaryDataOutputDto>(waterIncomeDiscountSummarys, @params);
            WaterIncomeDiscountSummaryHeaderOutputDto waterIncomeDiscountHeader = new WaterIncomeDiscountSummaryHeaderOutputDto()
            {
                Title = reportTitle,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterIncomeDiscountData.Count(),
                CustomerCount = waterIncomeDiscountData.Count(),

                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromConsumption = input.FromConsumption,
                ToConsumption = input.ToConsumption,

                SumBillCount = waterIncomeDiscountData.Sum(w => w.BillCount),
                SumTransactionCount = waterIncomeDiscountData.Sum(w => w.TransactionCount),
                SumSewageConsumption = waterIncomeDiscountData.Sum(w => w.SewageConsumption),
                SumConsumption = waterIncomeDiscountData.Sum(w => w.Consumption),
                SumConsumptionAverage = waterIncomeDiscountData.Sum(w => w.ConsumptionAverage),
                SumDuration = waterIncomeDiscountData.Sum(w => w.Duration),
                SumItemsOff = waterIncomeDiscountData.Sum(w => w.SumItemsOff),
                SumBillUnitCounts = waterIncomeDiscountData.Sum(w => w.BillUnitCounts),
                SumOffWater = waterIncomeDiscountData.Sum(w => w.SumOffWater),
                SumItemOff1 = waterIncomeDiscountData.Sum(w => w.ItemOff1),
                SumItemOff2 = waterIncomeDiscountData.Sum(w => w.ItemOff2),
                SumItemOff3 = waterIncomeDiscountData.Sum(w => w.ItemOff3),
                SumItemOff4 = waterIncomeDiscountData.Sum(w => w.ItemOff4),
                SumItemOff5 = waterIncomeDiscountData.Sum(w => w.ItemOff5),
                SumItemOff6 = waterIncomeDiscountData.Sum(w => w.ItemOff6),
                SumItemOff7 = waterIncomeDiscountData.Sum(w => w.ItemOff7),
                SumItemOff8 = waterIncomeDiscountData.Sum(w => w.ItemOff8),
                SumItemOff9 = waterIncomeDiscountData.Sum(w => w.ItemOff9),
                SumItemOff10 = waterIncomeDiscountData.Sum(w => w.ItemOff10),
                SumItemOff11 = waterIncomeDiscountData.Sum(w => w.ItemOff11),
                SumItemOff12 = waterIncomeDiscountData.Sum(w => w.ItemOff12),
                SumItemOff13 = waterIncomeDiscountData.Sum(w => w.ItemOff13),
                SumItemOff14 = waterIncomeDiscountData.Sum(w => w.ItemOff14),
                SumItemOff15 = waterIncomeDiscountData.Sum(w => w.ItemOff15),
                SumItemOff16 = waterIncomeDiscountData.Sum(w => w.ItemOff16),
                SumItemOff17 = waterIncomeDiscountData.Sum(w => w.ItemOff17),
                SumItemOff18 = waterIncomeDiscountData.Sum(w => w.ItemOff18),
                BillUnit = waterIncomeDiscountData.Sum(w => w.BillUnit),
                TotalUnit = waterIncomeDiscountData.Sum(w => w.TotalUnit),
            };

            var result = new ReportOutput<WaterIncomeDiscountSummaryHeaderOutputDto, WaterIncomeDiscountSummaryDataOutputDto>(reportTitle, waterIncomeDiscountHeader, waterIncomeDiscountData);
            return result;
        }
    }
}
