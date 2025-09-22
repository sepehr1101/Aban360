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
    internal sealed class WaterIncomeAndConsumptionDetailQueryService : AbstractBaseConnection, IWaterIncomeAndConsumptionDetailQueryService
    {
        public WaterIncomeAndConsumptionDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto>> Get(WaterIncomeAndConsumptionDetailInputDto input)
        {
            string waterIncomeAndConsumptionDetails = GetWaterIncomeAndConsumptionDetailQuery(input.ZoneIds.Any(), input.UsageIds.Any());
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
                zoneIds = input.ZoneIds
            };
            IEnumerable<WaterIncomeAndConsumptionDetailDataOutputDto> waterIncomeAndConsumptionData = await _sqlReportConnection.QueryAsync<WaterIncomeAndConsumptionDetailDataOutputDto>(waterIncomeAndConsumptionDetails, @params);
            WaterIncomeAndConsumptionDetailHeaderOutputDto waterIncomeAndConsumptionHeader = new WaterIncomeAndConsumptionDetailHeaderOutputDto()
            {
                Title= ReportLiterals.WaterIncomeAndConsumptionDetail,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterIncomeAndConsumptionData.Count(),

                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                FromConsumption = input.FromConsumption,
                ToConsumption = input.ToConsumption,

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

            var result = new ReportOutput<WaterIncomeAndConsumptionDetailHeaderOutputDto, WaterIncomeAndConsumptionDetailDataOutputDto>(ReportLiterals.WaterIncomeAndConsumptionDetail, waterIncomeAndConsumptionHeader, waterIncomeAndConsumptionData);
            return result;
        }

        private string GetWaterIncomeAndConsumptionDetailQuery(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty;

            return @$"use CustomerWarehouse
					Select
						b.ZoneTitle,
						TRIM(b.BillId) as BillId,
						b.UsageTitle,
						b.ReadingNumber,
						b.Consumption,
						b.ConsumptionAverage,
						b.WaterDiameterTitle as MeterDiameterTitle,
						b.BranchType AS BranchType,	
                        c.DeletionStateTitle AS UseStateTitle,
						(b.CommercialCount+b.DomesticCount+b.OtherCount) as BillUnitCounts,
						b.Duration,
						b.SumItems,
						b.Item1 ,
						b.Item2,
						b.Item3,
						b.Item4,
						b.Item5,
						b.Item6,
						b.Item7,
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
					Join [CustomerWarehouse].dbo.Clients c
						On b.BillId=c.BillId
					Where 
						(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
						(@fromConsumption IS NULL OR
						@toConsumption IS NULL OR
						b.Consumption BETWEEn @fromConsumption AND @toConsumption) AND
						(@fromAmount IS NULL OR
						@toAmount IS NULL OR
						b.SumItems BETWEEN @fromAmount AND @toAmount) AND
						b.TypeCode IN @typeCodes AND
                        c.ToDayJalali IS NULL
						{usageQuery}
						{zoneQuery}";
        }
    }
}
