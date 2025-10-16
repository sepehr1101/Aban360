using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterIncomeAndConsumptionSummaryQueryService : AbstractBaseConnection, IWaterIncomeAndConsumptionSummaryQueryService
    {
        public WaterIncomeAndConsumptionSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto>> Get(WaterIncomeAndConsumptionSummaryInputDto input)
        {
            string waterIncomeAndConsumptionSummarys = GetWaterIncomeAndConsumptionSummaryQuery(input.ZoneIds.HasValue(), input.UsageIds.HasValue(), input.BranchTypeIds.HasValue(), input.EnumInput);
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                fromConsumption = input.FromConsumption,
                toConsumption = input.ToConsumption,

                fromAmount = input.FromAmount,
                toAmount = input.ToAmount,

                typeCodes = input.IsNet ? new[] { 1, 3, 4, 5 } : new[] { 1 },

                usageIds = input.UsageIds,
                zoneIds = input.ZoneIds,
                branchTypeIds = input.BranchTypeIds,
            };
            IEnumerable<WaterIncomeAndConsumptionSummaryDataOutputDto> waterIncomeAndConsumptionData = await _sqlReportConnection.QueryAsync<WaterIncomeAndConsumptionSummaryDataOutputDto>(waterIncomeAndConsumptionSummarys, @params);
            WaterIncomeAndConsumptionSummaryHeaderOutputDto waterIncomeAndConsumptionHeader = new WaterIncomeAndConsumptionSummaryHeaderOutputDto()
            {
                Title = ReportLiterals.WaterIncomeAndConsumptionSummary,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterIncomeAndConsumptionData.Count(),
                CustomerCount = waterIncomeAndConsumptionData.Count(),

                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromConsumption = input.FromConsumption,
                ToConsumption = input.ToConsumption,

                SumBillCount = waterIncomeAndConsumptionData.Sum(w => w.BillCount),
                SumConsumption = waterIncomeAndConsumptionData.Sum(w => w.Consumption),
                SumConsumptionAverage = waterIncomeAndConsumptionData.Sum(w => w.ConsumptionAverage),
                SumDuration = waterIncomeAndConsumptionData.Sum(w => w.Duration),
                SumItems = waterIncomeAndConsumptionData.Sum(w => w.SumItems),
                SumBillUnitCounts = waterIncomeAndConsumptionData.Sum(w => w.BillUnitCounts),
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

            };

            var result = new ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto>(ReportLiterals.WaterIncomeAndConsumptionSummary, waterIncomeAndConsumptionHeader, waterIncomeAndConsumptionData);
            return result;
        }

        private string GetWaterIncomeAndConsumptionSummaryQuery(bool hasZone, bool hasUsage, bool hasBranchType, WaterIncomeAndConsumptionSummaryEnum enumState)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty;
            string branchTypeQuery = hasBranchType ? "AND b.BranchTypeId IN @branchTypeIds" : string.Empty;

            string groupKey = GetEnumQuery(enumState);

            return @$"use CustomerWarehouse
					Select
						{groupKey} as GroupKey,
						Count(1) as BillCount,
						SUM(b.Consumption) as Consumption,
						AVG(b.ConsumptionAverage) as ConsumptionAverage,
						SUM(b.Duration) as Duration,
						SUM(b.SumItems) as SumItems,
						SUM(b.CommercialCount+DomesticCount+OtherCount) as BillUnitCounts,
						SUM(b.Item1) as Item1,
						SUM(b.Item2) as Item2,
						SUM(b.Item3) as Item3,
						SUM(b.Item4) as Item4,
						SUM(b.Item5) as Item5,
						SUM(b.Item6) as Item6,
						SUM(b.Item7) as Item7,
						SUM(b.Item7) as Item7,
						SUM(b.Item8) as Item8,
						SUM(b.Item9) as Item9,
						SUM(b.Item10) as Item10,
						SUM(b.Item11) as Item11,
						SUM(b.Item12) as Item12,
						SUM(b.Item13) as Item13,
						SUM(b.Item14) as Item14,
						SUM(b.Item15) as Item15,
						SUM(b.Item16) as Item16,
						SUM(b.Item17) as Item17,
						SUM(b.Item18) as Item18 
					From [CustomerWarehouse].dbo.Bills b
					Where 
						(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
						(@fromConsumption IS NULL OR
						@toConsumption IS NULL OR
						b.Consumption BETWEEn @fromConsumption AND @toConsumption) AND
						(@fromAmount IS NULL OR
						@toAmount IS NULL OR
						b.SumItems BETWEEN @fromAmount AND @toAmount) AND
						b.TypeCode IN @typeCodes  
						{usageQuery}
						{zoneQuery}
                        {branchTypeQuery}
					Group BY {groupKey}";
        }

        private string GetEnumQuery(WaterIncomeAndConsumptionSummaryEnum enumState)
        {
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.AverageConsumption)
                return "Ceiling(b.ConsumptionAverage)";
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.RegisterDay)
                return "b.RegisterDay";
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.Zone)
                return "b.ZoneTitle";
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.Usage)
                return "b.UsageTitle";

            return "ZoneTitle";
        }
    }
}
