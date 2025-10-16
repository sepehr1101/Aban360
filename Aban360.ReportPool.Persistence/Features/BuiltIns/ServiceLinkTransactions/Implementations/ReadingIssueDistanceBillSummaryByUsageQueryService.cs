using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ReadingIssueDistanceBillSummaryByUsageQueryService : ReadingIssueDistanceBillBase, IReadingIssueDistanceBillSummaryByUsageQueryService
    {
        public ReadingIssueDistanceBillSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>> GetInfo(ReadingIssueDistanceBillInputDto input)
        {
            string reportTitle = ReportLiterals.ReadingIssueDistanceBillSummary + ReportLiterals.ByUsage;
            string query = GetGroupedQuery(GroupingFields.UsageTitle);

            IEnumerable<ReadingIssueDistanceBillSummryDataOutputDto> readingIssueDistanceData = await _sqlReportConnection.QueryAsync<ReadingIssueDistanceBillSummryDataOutputDto>(query, input, null, 180);
            ReadingIssueDistanceBillHeaderOutputDto readingIssueDistanceHeader = new ReadingIssueDistanceBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = readingIssueDistanceData is not null && readingIssueDistanceData.Any() ? readingIssueDistanceData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title=reportTitle,

                BillCount = readingIssueDistanceData.Sum(i => i.BillCount),
                SumCommercialUnit = readingIssueDistanceData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = readingIssueDistanceData.Sum(i => i.DomesticUnit),
                SumOtherUnit = readingIssueDistanceData.Sum(i => i.OtherUnit),
                TotalUnit = readingIssueDistanceData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>(reportTitle, readingIssueDistanceHeader, readingIssueDistanceData);

            return result;
        }
    }
}
