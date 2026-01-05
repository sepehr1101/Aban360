using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WaterIncomeAndConsumptionDetailQueryService : AbstractBaseConnection, IWaterIncomeAndConsumptionDetailQueryService
    {
        public WaterIncomeAndConsumptionDetailQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto>> Get(WaterIncomeAndConsumptionDetailInputDto input)
        {
            string reportTitle = ReportLiterals.WaterIncomeAndConsumptionDetail + GetIsZoneOrVillageTitle(input.ZoneIds);
            string waterIncomeAndConsumptionDetails = GetWaterIncomeAndConsumptionDetailQuery(input.ZoneIds.HasValue(), input.UsageIds.HasValue(), input.BranchTypeIds.HasValue());
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

                typeCodes = input.IsNet ? new[] { 1, 3, 4, 5 } : new[] { 1 },

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
        private string GetWaterIncomeAndConsumptionDetailQuery(bool hasZone, bool hasUsage, bool hasBranchType)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty;
            string branchTypeQuery = hasBranchType ? "AND b.BranchTypeId IN @branchTypeIds" : string.Empty;

            return @$"use CustomerWarehouse
					Select
						b.ZoneTitle,
						TRIM(b.BillId) as BillId,
						b.UsageTitle,
						b.ReadingNumber,
						Case When b.UsageId IN (1,3) AND b.BranchTypeId NOT IN (4) Then b.Consumption*0.7 Else b.Consumption End SewageConsumption,
						b.Consumption,
						b.ConsumptionAverage,
						b.WaterDiameterTitle as MeterDiameterTitle,
						b.BranchType AS BranchType,	
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
					Where 
						(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
						(@fromConsumption IS NULL OR
						@toConsumption IS NULL OR
						b.Consumption BETWEEn @fromConsumption AND @toConsumption) AND
						(@fromAmount IS NULL OR
						@toAmount IS NULL OR
						b.SumItems BETWEEN @fromAmount AND @toAmount) AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						b.TypeCode IN @typeCodes
						{usageQuery}
						{zoneQuery}
                        {branchTypeQuery}";
        }
    }
}
