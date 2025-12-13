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
    internal sealed class ConsumptionAverageAnalysisQueryService : AbstractBaseConnection, IConsumptionAverageAnalysisQueryService
    {
        public ConsumptionAverageAnalysisQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto>> Get(ConsumptionAverageAnalysisInputDto input)
        {
            string reportTitle = ReportLiterals.LowWorkingMeter;
            string groupField = input.IsZoneGroup ? ReportLiterals.ZoneTitle : ReportLiterals.UsageTitle;
            string query = GetDataQuery(groupField, input.Values);
            IEnumerable<ConsumptionAverageAnalysisDataOutputDto> data = await _sqlReportConnection.QueryAsync<ConsumptionAverageAnalysisDataOutputDto>(query, input);
            ConsumptionAverageAnalysisHeaderOutputDto header = new()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = data.Count(),
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title = reportTitle,
                Values = GetBarChartValue(data)
            };

            ReportOutput<ConsumptionAverageAnalysisHeaderOutputDto, ConsumptionAverageAnalysisDataOutputDto> result = new(reportTitle, header, data.OrderBy(c => c.ItemTitle).ThenBy(c => c.FromToConsumption));
            return result;
        }
        private IEnumerable<ConsumptionAverageAnalysisHeaderValueDto> GetBarChartValue(IEnumerable<ConsumptionAverageAnalysisDataOutputDto> values)
        {
            return values
                  .GroupBy(v => v.FromToConsumption)
                  .Select(s => new ConsumptionAverageAnalysisHeaderValueDto()
                  {
                      FromToConsumption = s.Key,
                      Count = s.Sum(c => c.Count),
                      Unit = s.Sum(c => c.Unit)
                  })
                  .OrderBy(c => c.FromToConsumption)
                  .ToList();
        }
        private string GetDataQuery(string groupField, ICollection<ContractualAndOlgooLevelValuesInputDto> values)
        {
            string query = @$";with cte as(
								Select
									MAX(t46.C2) AS RegionTitle,
									MAX(b.ZoneTitle) ZoneTitle,
									MAX(b.CustomerNumber) CustomerNumber,
									AVG(b.consumptionAverage) consumptionAverage,
									AVG(b.DomesticCount+b.CommercialCount+b.OtherCount) UnitAverage
								From CustomerWarehouse.dbo.Bills b
								Left Join CustomerWarehouse.dbo.Clients c
									On b.zoneId=c.zoneId AND b.CustomerNumber=c.CustomerNumber
								Join [Db70].dbo.T51 t51
									On t51.C0=b.ZoneId
								Join [Db70].dbo.T46 t46
									On t51.C1=t46.C0
								Where
									c.ToDayJalali IS NULL AND
									b.RegisterDay Between @FromDateJalali AND @ToDateJalali AND
									b.ZoneId IN @ZoneIds AND
									b.UsageId IN @UsageIds AND 
									b.TypeCode IN (1,3,4,5) 
								Group by b.BillId 
							)";
            return query + GetSelectQuery(values, groupField);
        }
        private string GetSelectQuery(ICollection<ContractualAndOlgooLevelValuesInputDto> Values, string groupField)
        {
            string query = string.Empty;
            string selectBase = @" Select 
								   		{0} as ItemTitle,
                                        '{1}-{2}' as FromToConsumption,
								   		COUNT(Case When (ConsumptionAverage BETWEEN {3} AND {2}) Then 1 Else null END )as Count,
								   		SUM(Case When (ConsumptionAverage BETWEEN {3} AND {2}) Then UnitAverage Else null END )as Unit
								   From cte
								   Where ConsumptionAverage BETWEEN {3} AND {2}
								   Group BY {0} ";

            for (int i = 0; i < Values.Count; i++)
            {
                ContractualAndOlgooLevelValuesInputDto item = Values.ElementAt(i);
                float fromValue = item.FromValue == 0 ? -999999 : item.FromValue;
                string singleSelect = string.Format(selectBase, groupField, item.FromValue, item.ToValue, fromValue);
                query += singleSelect;
                if (i != Values.Count - 1)
                {
                    string union = " Union All ";
                    query += union;
                }
            }
            return query;
        }
    }
}
