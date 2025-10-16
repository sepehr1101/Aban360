using Aban360.Common.BaseEntities;
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
    internal sealed class UnreadSummaryByUsageQueryService : UnreadBase, IUnreadSummaryByUsageQueryService
    {
        public UnreadSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string reportTitle = ReportLiterals.UnreadSummary + ReportLiterals.ByUsage;
            string query = GetGroupedQuery(input.ZoneIds.HasValue(), GroupingFields.UsageTitle);

            IEnumerable<UnreadSummaryDataOutputDto> UnreadSummaryByUsageData = await _sqlReportConnection.QueryAsync<UnreadSummaryDataOutputDto>(query, input, null, 180);
            UnreadSummaryHeaderOutputDto UnreadSummaryByUsageHeader = new UnreadSummaryHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromPeriodCount = input.FromPeriodCount,
                ToPeriodCount = input.ToPeriodCount,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = UnreadSummaryByUsageData is not null && UnreadSummaryByUsageData.Any() ? UnreadSummaryByUsageData.Count() : 0,
                Title=reportTitle,

                SumBarrier = UnreadSummaryByUsageData.Sum(u => u.Barrier),
                SumClosed = UnreadSummaryByUsageData.Sum(u => u.Closed),
                SumCommercialUnit = UnreadSummaryByUsageData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = UnreadSummaryByUsageData.Sum(i => i.DomesticUnit),
                SumOtherUnit = UnreadSummaryByUsageData.Sum(i => i.OtherUnit),
                TotalUnit = UnreadSummaryByUsageData.Sum(i => i.TotalUnit),
                CustomerCount = UnreadSummaryByUsageData.Sum(i => i.CustomerCount),
                Count0 = UnreadSummaryByUsageData?.Sum(i => i.Count0) ?? 0,
                Count1 = UnreadSummaryByUsageData?.Sum(i => i.Count1) ?? 0,
                Count2 = UnreadSummaryByUsageData?.Sum(i => i.Count2) ?? 0,
                CountMore = UnreadSummaryByUsageData?.Sum(i => i.CountMore) ?? 0,

            };

            var result = new ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto>(reportTitle, UnreadSummaryByUsageHeader, UnreadSummaryByUsageData);
            return result;
        }
    }
}
