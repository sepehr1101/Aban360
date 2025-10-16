using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class EmptyUnitByBillIdUsageGroupingQueryService : EmptyUnitByBillBase, IEmptyUnitByBillIdUsageGroupingQueryService
    {
        public EmptyUnitByBillIdUsageGroupingQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdSummaryDataOutputDto>> Get(EmptyUnitInputDto input)
        {
            string reportTitle = ReportLiterals.EmptyUnitByBillSummary + ReportLiterals.ByUsage;
            string query = GetGroupedQuery(input.ZoneIds.HasValue(), input.UsageSellIds.HasValue(), GroupingFields.UsageTitle);

            IEnumerable<EmptyUnitByBillIdSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<EmptyUnitByBillIdSummaryDataOutputDto>(query, input);
            EmptyUnitByBillIdSummaryHeaderOutputDto RequestHeader = new EmptyUnitByBillIdSummaryHeaderOutputDto()
            {
                FromEmptyUnit = input.FromEmptyUnit,
                ToEmptyUnit = input.ToEmptyUnit,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,

                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = RequestData is not null && RequestData.Any() ? RequestData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = RequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = RequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = RequestData.Sum(i => i.OtherUnit),
                TotalUnit = RequestData.Sum(i => i.TotalUnit),
                EmptyUnit = RequestData.Sum(i => i.EmptyUnit),
                CustomerCount = RequestData.Sum(i => i.CustomerCount),
            };
            var result = new ReportOutput<EmptyUnitByBillIdSummaryHeaderOutputDto, EmptyUnitByBillIdSummaryDataOutputDto>
                (reportTitle,
                RequestHeader,
                RequestData);

            return result;
        }
    }
}
