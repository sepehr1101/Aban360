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
            string reportTitle = ReportLiterals.WaterIncomeAndConsumptionSummary + GetIsZoneOrVillageTitle(input.ZoneIds);
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
                Title = reportTitle,
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
                SumSewageConsumption = waterIncomeAndConsumptionData.Sum(w => w.SewageConsumption),
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

            var result = new ReportOutput<WaterIncomeAndConsumptionSummaryHeaderOutputDto, WaterIncomeAndConsumptionSummaryDataOutputDto>(reportTitle, waterIncomeAndConsumptionHeader, waterIncomeAndConsumptionData);
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
        private string GetWaterIncomeAndConsumptionSummaryQuery(bool hasZone, bool hasUsage, bool hasBranchType, WaterIncomeAndConsumptionSummaryEnum enumState)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty;
            string branchTypeQuery = hasBranchType ? "AND b.BranchTypeId IN @branchTypeIds" : string.Empty;

            string groupKey = GetEnumQuery(enumState);

            return @$";With cte as(
                    	Select
                    		b.ZoneTitle,
                    		TRIM(b.BillId) as BillId,
                    		t41.C1 as UsageTitle, 
                    		b.ReadingNumber,
                    		(b.CommercialCount+b.DomesticCount+b.OtherCount) as BillUnitCounts,
                    		Case When b.UsageId IN (1,3) AND b.BranchTypeId NOT IN (4) Then b.Consumption*0.7 Else b.Consumption End SewageConsumption,
                    		b.Consumption,
                    		b.ConsumptionAverage,
                    		b.WaterDiameterTitle as MeterDiameterTitle,
                    		b.BranchType AS BranchType,	
                            b.RegisterDay,
                    		b.Duration,
                    		b.SumItems,
                    		b.Item1 ,
                    		b.Item2,
                    		b.Item3,
                    		b.Item4,
                    		b.Item5,
                    		b.Item6,
                    		b.Item7,
                    		b.Item8,
                    		b.Item9,
                    		b.Item10,
                    		b.Item11,
                    		b.Item12,
                    		b.Item13,
                    		b.Item14,
                    		b.Item15,
                    		b.Item16,
                    		b.Item17,
                    		b.Item18
                    From [CustomerWarehouse].dbo.Bills b
                    Join [Db70].dbo.T41 t41
                    	ON b.UsageId=t41.C0
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
                    )
                    Select
                    	{groupKey} as GroupKey,
                    	Count(1) as BillCount,
                    	SUM(SewageConsumption) as SewageConsumption,
                    	SUM(Consumption) as Consumption,
                    	AVG(ConsumptionAverage) as ConsumptionAverage,
                    	SUM(Duration) as Duration,
                    	SUM(SumItems) as SumItems,
                    	SUM(BillUnitCounts) as BillUnitCounts,
                    	SUM(Item1) as Item1,
                    	SUM(Item2) as Item2,
                    	SUM(Item3) as Item3,
                    	SUM(Item4) as Item4,
                    	SUM(Item5) as Item5,
                    	SUM(Item6) as Item6,
                    	SUM(Item7) as Item7,
                    	SUM(Item7) as Item7,
                    	SUM(Item8) as Item8,
                    	SUM(Item9) as Item9,
                    	SUM(Item10) as Item10,
                    	SUM(Item11) as Item11,
                    	SUM(Item12) as Item12,
                    	SUM(Item13) as Item13,
                    	SUM(Item14) as Item14,
                    	SUM(Item15) as Item15,
                    	SUM(Item16) as Item16,
                    	SUM(Item17) as Item17,
                    	SUM(Item18) as Item18 
                    From cte
                    Group By {groupKey}";
        }

        private string GetEnumQuery(WaterIncomeAndConsumptionSummaryEnum enumState)
        {
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.AverageConsumption)
                return "Ceiling(ConsumptionAverage)";
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.RegisterDay)
                return "RegisterDay";
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.Zone)
                return "ZoneTitle";
            if (enumState == WaterIncomeAndConsumptionSummaryEnum.Usage)
                return "UsageTitle";

            return "ZoneTitle";
        }
    }
}
