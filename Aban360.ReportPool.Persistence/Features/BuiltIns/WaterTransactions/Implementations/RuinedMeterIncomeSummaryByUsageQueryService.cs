using Aban360.Common.BaseEntities;
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
    internal sealed class RuinedMeterIncomeSummaryByUsageQueryService : RuinedMeterIncomeBase, IRuinedMeterIncomeSummaryByUsageQueryService
    {
        public RuinedMeterIncomeSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>> GetInfo(RuinedMeterIncomeInputDto input)
        {
            string reportTitle = ReportLiterals.RuinedMeterIncomeSummary + ReportLiterals.ByUsage + ReportLiterals.ByZone;
            string query = GetGroupedQuery(GroupingFields.UsageTitle);
         
            IEnumerable<RuinedMeterIncomeSummaryDataOutputDto> ruinedMeterIncomeData = await _sqlReportConnection.QueryAsync<RuinedMeterIncomeSummaryDataOutputDto>(query, input);
            RuinedMeterIncomeHeaderOutputDto ruinedMeterIncomeHeader = new RuinedMeterIncomeHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = ruinedMeterIncomeData is not null && ruinedMeterIncomeData.Any() ? ruinedMeterIncomeData.Count() : 0,
                Title=reportTitle,

                TotalPayable = ruinedMeterIncomeData.Sum(x => x.Payable),
                TotalSumItems = ruinedMeterIncomeData.Sum(x => x.SumItems),
                SumCommercialUnit = ruinedMeterIncomeData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = ruinedMeterIncomeData.Sum(i => i.DomesticUnit),
                SumOtherUnit = ruinedMeterIncomeData.Sum(i => i.OtherUnit),
                TotalUnit = ruinedMeterIncomeData.Sum(i => i.TotalUnit),
                CustomerCount = ruinedMeterIncomeData.Sum(i => i.CustomerCount),
            };
            var result = new ReportOutput<RuinedMeterIncomeHeaderOutputDto, RuinedMeterIncomeSummaryDataOutputDto>(reportTitle, ruinedMeterIncomeHeader, ruinedMeterIncomeData);

            return result;
        }
    }
}
