using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class ContractualAndOlgooLevelQueryService : AbstractBaseConnection, IContractualAndOlgooLevelQueryService
    {
        public ContractualAndOlgooLevelQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ContractualAndOlgooLevelHeaderOutputDto, ContractualAndOlgooLevelDataOutputDto>> Get(ContractualAndOlgooLevelInputDto input)
        {
            string rangeValues = string.Join(",", input.Values.Select(v => $"({v.FromValue},{v.ToValue})"));

            string getOlgooLevelQueryString = GetOlgooLevelQuery(input.Inputs.ZoneIds.Any(), input.Inputs.UsageIds.Any(), rangeValues);
            string getContractualLevelQueryString = GetContractualLevelQuery(input.Inputs.ZoneIds.Any(), input.Inputs.UsageIds.Any(), rangeValues);
            string LevelQuery = input.IsConsumption ? getContractualLevelQueryString : getOlgooLevelQueryString;

            var @params = new
            {
                fromDate = input.Inputs.FromDateJalali,
                toDate = input.Inputs.ToDateJalali,

                fromConsumption = input.Inputs.FromConsumption,
                toConsumption = input.Inputs.ToConsumption,

                fromAmount = input.Inputs.FromAmount,
                toAmount = input.Inputs.ToAmount,

                typeCodes = input.Inputs.IsNet ? new[] { 1, 3, 4, 5 } : new[] { 1 },

                usageIds = input.Inputs.UsageIds,
                zoneIds = input.Inputs.ZoneIds,
            };
            IEnumerable<ContractualAndOlgooLevelDataOutputDto> levelData = await _sqlReportConnection.QueryAsync<ContractualAndOlgooLevelDataOutputDto>(LevelQuery, @params);
            ContractualAndOlgooLevelHeaderOutputDto levelHeader = new ContractualAndOlgooLevelHeaderOutputDto()
            {
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = levelData.Count(),

                FromDateJalali = input.Inputs.FromDateJalali,
                ToDateJalali = input.Inputs.ToDateJalali,
                FromAmount = input.Inputs.FromAmount,
                ToAmount = input.Inputs.ToAmount,
                FromConsumption = input.Inputs.FromConsumption,
                ToConsumption = input.Inputs.ToConsumption,

                SumBillCount = levelData.Sum(w => w.BillCount),
                SumConsumption = levelData.Sum(w => w.Consumption),
                ConsumptionAverage = levelData.Average(w => w.ConsumptionAverage),
                SumDuration = levelData.Sum(w => w.Duration),
                SumItems = levelData.Sum(w => w.SumItems),
                SumBillUnitCounts = levelData.Sum(w => w.BillUnitCounts),
                SumItem1 = levelData.Sum(w => w.Item1),
                SumItem2 = levelData.Sum(w => w.Item2),
                SumItem3 = levelData.Sum(w => w.Item3),
                SumItem4 = levelData.Sum(w => w.Item4),
                SumItem5 = levelData.Sum(w => w.Item5),
                SumItem6 = levelData.Sum(w => w.Item6),
                SumItem7 = levelData.Sum(w => w.Item7),
                SumItem8 = levelData.Sum(w => w.Item8),
                SumItem9 = levelData.Sum(w => w.Item9),
                SumItem10 = levelData.Sum(w => w.Item10),
                SumItem11 = levelData.Sum(w => w.Item11),
                SumItem12 = levelData.Sum(w => w.Item12),
                SumItem13 = levelData.Sum(w => w.Item13),
                SumItem14 = levelData.Sum(w => w.Item14),
                SumItem15 = levelData.Sum(w => w.Item15),
                SumItem16 = levelData.Sum(w => w.Item16),
                SumItem17 = levelData.Sum(w => w.Item17),
                SumItem18 = levelData.Sum(w => w.Item18),

            };

            var result = new ReportOutput<ContractualAndOlgooLevelHeaderOutputDto, ContractualAndOlgooLevelDataOutputDto>(ReportLiterals.ContractualAndOlgooLevel, levelHeader, levelData);
            return result;
        }

        private string GetOlgooLevelQuery(bool hasZone, bool hasUsage, string rangeValues)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty;

            return @$"Select
                        b.ZoneTitle,
				    	g.FromValue,
				    	g.ToValue,
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
				    Join [OldCalc].dbo.table1 t
				    	on t.town=b.ZoneId
				     JOIN (
                          VALUES {rangeValues}
                      ) AS g(FromValue, ToValue)
                          ON b.ConsumptionAverage>g.FromValue*t.olgo AND ConsumptionAverage<=g.ToValue*t.olgo
                    Where
				    	(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
				    	(@fromConsumption IS NULL OR
				    	@toConsumption IS NULL OR
				    	b.Consumption BETWEEn @fromConsumption AND @toConsumption) AND
				    	(@fromAmount IS NULL OR
				    	@toAmount IS NULL OR
				    	b.SumItems BETWEEN @fromAmount AND @toAmount)
				    	{usageQuery}
				        {zoneQuery}
				    Group by
				    	g.FromValue,
				    	g.ToValue,
				    	b.ZoneTitle";
        }
        private string GetContractualLevelQuery(bool hasZone, bool hasUsage, string rangeValues)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId IN @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty;

            return @$"Select
                        b.ZoneTitle,
				    	g.FromValue,
				    	g.ToValue,
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
				     JOIN (
                          VALUES {rangeValues}
                      ) AS g(FromValue, ToValue)
                        ON b.ConsumptionAverage >g.FromValue*b.ContractCapacity AND ConsumptionAverage<=g.ToValue*b.ContractCapacity
                    Where
				    	(b.RegisterDay BETWEEN @fromDate AND @toDate) AND
				    	(@fromConsumption IS NULL OR
				    	@toConsumption IS NULL OR
				    	b.Consumption BETWEEn @fromConsumption AND @toConsumption) AND
				    	(@fromAmount IS NULL OR
				    	@toAmount IS NULL OR
				    	b.SumItems BETWEEN @fromAmount AND @toAmount)
				    	{usageQuery}
				        {zoneQuery}
				    Group by
				    	g.FromValue,
				    	g.ToValue,
				    	b.ZoneTitle";
        }
    }
}
