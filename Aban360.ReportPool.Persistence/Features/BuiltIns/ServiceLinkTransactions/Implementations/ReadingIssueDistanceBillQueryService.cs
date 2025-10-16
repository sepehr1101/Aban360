using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ReadingIssueDistanceBillQueryService : ReadingIssueDistanceBillBase, IReadingIssueDistanceBillQueryService
    {
        public ReadingIssueDistanceBillQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillDataOutputDto>> GetInfo(ReadingIssueDistanceBillInputDto input)
        {
            string query = GetDetailQuery();

            IEnumerable<ReadingIssueDistanceBillDataOutputDto> readingIssueDistanceData = await _sqlReportConnection.QueryAsync<ReadingIssueDistanceBillDataOutputDto>(query, input, null, 180);
            ReadingIssueDistanceBillHeaderOutputDto readingIssueDistanceHeader = new ReadingIssueDistanceBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = readingIssueDistanceData is not null && readingIssueDistanceData.Any() ? readingIssueDistanceData.Count() : 0,
                BillCount = readingIssueDistanceData is not null && readingIssueDistanceData.Any() ? readingIssueDistanceData.Count() : 0,
                Title= ReportLiterals.ReadingIssueDistanceBillDetail,

                SumCommercialUnit = readingIssueDistanceData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = readingIssueDistanceData.Sum(i => i.DomesticUnit),
                SumOtherUnit = readingIssueDistanceData.Sum(i => i.OtherUnit),
                TotalUnit = readingIssueDistanceData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillDataOutputDto>(ReportLiterals.ReadingIssueDistanceBillDetail, readingIssueDistanceHeader, readingIssueDistanceData);

            return result;
        }
    }
}
