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
    internal sealed class WaterReturnSummaryQueryService : WaterReturnBase, IWaterReturnSummaryQueryService
    {
        public WaterReturnSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterReturnSummaryHeaderOutputDto, WaterReturnSummaryDataOutputDto>> Get(WaterReturnSummaryInputDto input)
        {
            string reportTitle = ReportLiterals.WaterReturnSummary + GetIsZoneOrVillageTitle(input.ZoneIds);
            string WaterReturnSummarys = GetSummaryQuery(false, input.ZoneIds.HasValue(), input.UsageIds.HasValue(), input.BranchTypeIds.HasValue(), input.EnumInput);

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                fromConsumption = input.FromConsumption,
                toConsumption = input.ToConsumption,

                fromAmount = input.FromAmount,
                toAmount = input.ToAmount,

                typeCodes = GetTypeCodes(input.type),

                usageIds = input.UsageIds,
                zoneIds = input.ZoneIds,
                branchTypeIds = input.BranchTypeIds,
                ReturnCauseIds = input.ReturnCauseIds
            };
            IEnumerable<WaterReturnSummaryDataOutputDto> WaterReturnData = await _sqlReportConnection.QueryAsync<WaterReturnSummaryDataOutputDto>(WaterReturnSummarys, @params);
            WaterReturnSummaryHeaderOutputDto WaterReturnHeader = new WaterReturnSummaryHeaderOutputDto()
            {
                Title = reportTitle,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = WaterReturnData.Count(),
                CustomerCount = WaterReturnData.Count(),

                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromConsumption = input.FromConsumption,
                ToConsumption = input.ToConsumption,

                SumBillCount = WaterReturnData.Sum(w => w.BillCount),
                SumTransactionCount = WaterReturnData.Sum(w => w.TransactionCount),
                SumSewageConsumption = WaterReturnData.Sum(w => w.SewageConsumption),
                SumConsumption = WaterReturnData.Sum(w => w.Consumption),
                SumConsumptionAverage = WaterReturnData.Sum(w => w.ConsumptionAverage),
                SumDuration = WaterReturnData.Sum(w => w.Duration),
                SumItems = WaterReturnData.Sum(w => w.SumItems),
                SumBillUnitCounts = WaterReturnData.Sum(w => w.BillUnitCounts),
                SumWater = WaterReturnData.Sum(w => w.SumWater),
                SumItem1 = WaterReturnData.Sum(w => w.Item1),
                SumItem2 = WaterReturnData.Sum(w => w.Item2),
                SumItem3 = WaterReturnData.Sum(w => w.Item3),
                SumItem4 = WaterReturnData.Sum(w => w.Item4),
                SumItem5 = WaterReturnData.Sum(w => w.Item5),
                SumItem6 = WaterReturnData.Sum(w => w.Item6),
                SumItem7 = WaterReturnData.Sum(w => w.Item7),
                SumItem8 = WaterReturnData.Sum(w => w.Item8),
                SumItem9 = WaterReturnData.Sum(w => w.Item9),
                SumItem10 = WaterReturnData.Sum(w => w.Item10),
                SumItem11 = WaterReturnData.Sum(w => w.Item11),
                SumItem12 = WaterReturnData.Sum(w => w.Item12),
                SumItem13 = WaterReturnData.Sum(w => w.Item13),
                SumItem14 = WaterReturnData.Sum(w => w.Item14),
                SumItem15 = WaterReturnData.Sum(w => w.Item15),
                SumItem16 = WaterReturnData.Sum(w => w.Item16),
                SumItem17 = WaterReturnData.Sum(w => w.Item17),
                SumItem18 = WaterReturnData.Sum(w => w.Item18),
                BillUnit = WaterReturnData.Sum(w => w.BillUnit),
                TotalUnit = WaterReturnData.Sum(w => w.TotalUnit),
            };

            var result = new ReportOutput<WaterReturnSummaryHeaderOutputDto, WaterReturnSummaryDataOutputDto>(reportTitle, WaterReturnHeader, WaterReturnData);
            return result;
        }
    }
}
