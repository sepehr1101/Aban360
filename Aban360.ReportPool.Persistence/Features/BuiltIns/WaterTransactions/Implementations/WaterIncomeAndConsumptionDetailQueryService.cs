using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterIncomeAndConsumptionDetailQueryService : WaterIncomeAndConsumptionBase, IWaterIncomeAndConsumptionDetailQueryService
    {
        public WaterIncomeAndConsumptionDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto>> Get(WaterIncomeAndConsumptionDetailInputDto input)
        {
            string reportTitle = ReportLiterals.WaterIncomeAndConsumptionDetail + GetIsZoneOrVillageTitle(input.ZoneIds);
            string waterIncomeAndConsumptionDetails = GetDetailQuery(input.ZoneIds.HasValue(), input.UsageIds.HasValue(), input.BranchTypeIds.HasValue());
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

                usageIds = input.UsageIds,
                zoneIds = input.ZoneIds,
                branchTypeIds = input.BranchTypeIds,
            };
            IEnumerable<WaterIncomeAndConsumptionDetailDataOutputDto> waterIncomeAndConsumptionData = await _sqlReportConnection.QueryAsync<WaterIncomeAndConsumptionDetailDataOutputDto>(waterIncomeAndConsumptionDetails, @params);
            WaterIncomeAndConsumptionDetailHeaderOutputDto waterIncomeAndConsumptionHeader = new WaterIncomeAndConsumptionDetailHeaderOutputDto()
            {
                Title = reportTitle,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                SumBillCount = waterIncomeAndConsumptionData.Count(),
                RecordCount = waterIncomeAndConsumptionData.Count(),
                CustomerCount = waterIncomeAndConsumptionData.GroupBy(r => r.BillId).Distinct().Count(),

                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromConsumption = input.FromConsumption,
                ToConsumption = input.ToConsumption,

                SumSewageConsumption = waterIncomeAndConsumptionData.Sum(w => w.SewageConsumption),
                SumConsumption = waterIncomeAndConsumptionData.Sum(w => w.Consumption),
                SumConsumptionAverage = waterIncomeAndConsumptionData.Sum(w => w.ConsumptionAverage),
                SumDuration = waterIncomeAndConsumptionData.Sum(w => w.Duration),
                SumItems = waterIncomeAndConsumptionData.Sum(w => w.SumItems),
                SumBillUnitCounts = waterIncomeAndConsumptionData.Sum(w => w.BillUnitCounts),
                SumWater = waterIncomeAndConsumptionData.Sum(w => w.SumWater),
                SumItem1 = waterIncomeAndConsumptionData.Sum(w => w.Item1),
                SumItem2 = waterIncomeAndConsumptionData.Sum(w => w.Item2),
                SumItem3 = waterIncomeAndConsumptionData.Sum(w => w.Item3),
                SumItem4 = waterIncomeAndConsumptionData.Sum(w => w.Item4),
                SumItem5 = waterIncomeAndConsumptionData.Sum(w => w.Item5),
                SumItem6 = waterIncomeAndConsumptionData.Sum(w => w.Item6),
                SumItem7 = waterIncomeAndConsumptionData.Sum(w => w.Item7),
                SumItem8 = waterIncomeAndConsumptionData.Sum(w => w.Item8),
                SumItem9 = waterIncomeAndConsumptionData.Sum(w => w.Item9),
                SumItem10 = waterIncomeAndConsumptionData.Sum(w => w.Item10),
                SumItem11 = waterIncomeAndConsumptionData.Sum(w => w.Item11),
                SumItem12 = waterIncomeAndConsumptionData.Sum(w => w.Item12),
                SumItem13 = waterIncomeAndConsumptionData.Sum(w => w.Item13),
                SumItem14 = waterIncomeAndConsumptionData.Sum(w => w.Item14),
                SumItem15 = waterIncomeAndConsumptionData.Sum(w => w.Item15),
                SumItem16 = waterIncomeAndConsumptionData.Sum(w => w.Item16),
                SumItem17 = waterIncomeAndConsumptionData.Sum(w => w.Item17),
                SumItem18 = waterIncomeAndConsumptionData.Sum(w => w.Item18),
                BillUnit = waterIncomeAndConsumptionData.Sum(w => w.BillUnit),
                TotalUnit = waterIncomeAndConsumptionData.Sum(w => w.TotalUnit),
            };

            var result = new ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto>(reportTitle, waterIncomeAndConsumptionHeader, waterIncomeAndConsumptionData);
            return result;
        }
        private string GetIsZoneOrVillageTitle(IEnumerable<int> zoneIds)
        {
            int villageId = 140000;

            bool allVillages = zoneIds.All(z => z > villageId);
            bool anyVillage = zoneIds.Any(z => z > villageId);

            if (allVillages)
                return ReportLiterals.WithVillage;

            if (!anyVillage)
                return ReportLiterals.WithZone;

            return string.Empty;
        }
    }
}