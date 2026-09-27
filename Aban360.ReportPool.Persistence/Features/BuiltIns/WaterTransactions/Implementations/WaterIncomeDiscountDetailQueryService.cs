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
    internal sealed class WaterIncomeDiscountDetailQueryService : WaterIncomeDiscountBase, IWaterIncomeDiscountDetailQueryService
    {
        public WaterIncomeDiscountDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterIncomeDiscountDetailHeaderOutputDto, WaterIncomeDiscountDetailDataOutputDto>> Get(WaterIncomeDiscountDetailInputDto input)
        {
            string reportTitle = ReportLiterals.WaterIncomeDiscountDetail + GetIsZoneOrVillageTitle(input.ZoneIds);
            string WaterIncomeDiscountDetails = GetDetailQuery(input.ZoneIds.HasValue(),input.DiscountCauseId);
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromConsumption = input.FromConsumption,
                toConsumption = input.ToConsumption,

                fromAmount = input.FromAmount,
                toAmount = input.ToAmount,

                typeCodes = GetTypeCodes(input.type),

                zoneIds = input.ZoneIds,
            };
            IEnumerable<WaterIncomeDiscountDetailDataOutputDto> WaterIncomeDiscountData = await _sqlReportConnection.QueryAsync<WaterIncomeDiscountDetailDataOutputDto>(WaterIncomeDiscountDetails, @params);
            WaterIncomeDiscountDetailHeaderOutputDto WaterIncomeDiscountHeader = new WaterIncomeDiscountDetailHeaderOutputDto()
            {
                Title = reportTitle,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                SumBillCount = WaterIncomeDiscountData.Count(),
                RecordCount = WaterIncomeDiscountData.Count(),
                CustomerCount = WaterIncomeDiscountData.GroupBy(r => r.BillId).Distinct().Count(),

                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromConsumption = input.FromConsumption,
                ToConsumption = input.ToConsumption,

                SumSewageConsumption = WaterIncomeDiscountData.Sum(w => w.SewageConsumption),
                SumConsumption = WaterIncomeDiscountData.Sum(w => w.Consumption),
                SumConsumptionAverage = WaterIncomeDiscountData.Sum(w => w.ConsumptionAverage),
                SumDuration = WaterIncomeDiscountData.Sum(w => w.Duration),
                SumItemsOff = WaterIncomeDiscountData.Sum(w => w.SumItemsOff),
                SumBillUnitCounts = WaterIncomeDiscountData.Sum(w => w.BillUnitCounts),
                SumOffWater = WaterIncomeDiscountData.Sum(w => w.SumOffWater),
                SumItemOff1 = WaterIncomeDiscountData.Sum(w => w.ItemOff1),
                SumItemOff2 = WaterIncomeDiscountData.Sum(w => w.ItemOff2),
                SumItemOff3 = WaterIncomeDiscountData.Sum(w => w.ItemOff3),
                SumItemOff4 = WaterIncomeDiscountData.Sum(w => w.ItemOff4),
                SumItemOff5 = WaterIncomeDiscountData.Sum(w => w.ItemOff5),
                SumItemOff6 = WaterIncomeDiscountData.Sum(w => w.ItemOff6),
                SumItemOff7 = WaterIncomeDiscountData.Sum(w => w.ItemOff7),
                SumItemOff8 = WaterIncomeDiscountData.Sum(w => w.ItemOff8),
                SumItemOff9 = WaterIncomeDiscountData.Sum(w => w.ItemOff9),
                SumItemOff10 = WaterIncomeDiscountData.Sum(w => w.ItemOff10),
                SumItemOff11 = WaterIncomeDiscountData.Sum(w => w.ItemOff11),
                SumItemOff12 = WaterIncomeDiscountData.Sum(w => w.ItemOff12),
                SumItemOff13 = WaterIncomeDiscountData.Sum(w => w.ItemOff13),
                SumItemOff14 = WaterIncomeDiscountData.Sum(w => w.ItemOff14),
                SumItemOff15 = WaterIncomeDiscountData.Sum(w => w.ItemOff15),
                SumItemOff16 = WaterIncomeDiscountData.Sum(w => w.ItemOff16),
                SumItemOff17 = WaterIncomeDiscountData.Sum(w => w.ItemOff17),
                SumItemOff18 = WaterIncomeDiscountData.Sum(w => w.ItemOff18),
                BillUnit = WaterIncomeDiscountData.Sum(w => w.BillUnit),
                TotalUnit = WaterIncomeDiscountData.Sum(w => w.TotalUnit),
            };

            var result = new ReportOutput<WaterIncomeDiscountDetailHeaderOutputDto, WaterIncomeDiscountDetailDataOutputDto>(reportTitle, WaterIncomeDiscountHeader, WaterIncomeDiscountData);
            return result;
        }
    }
}
