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
    internal sealed class ReadingListSummaryByUsageQueryService : ReadingListBase, IReadingListSummaryByUsageQueryService
    {
        public ReadingListSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>> GetInfo(ReadingListInputDto input)
        {
            string reportTitle = ReportLiterals.ReadingListSummary + ReportLiterals.ByUsage;
            string query = GetGroupedQuery(GroupingFields.UsageTitle);
            
            IEnumerable<ReadingListSummaryDataOutputDto> readingListData = await _sqlReportConnection.QueryAsync<ReadingListSummaryDataOutputDto>(query, input);
            ReadingListHeaderOutputDto modifiedBillsHeader = new ReadingListHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = readingListData is not null && readingListData.Any() ? readingListData.Count() : 0,
                CustomerCount = readingListData?.Sum(x => x.ReadingCount) ?? 0,
                Title=reportTitle,
            };

            var result = new ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>(reportTitle, modifiedBillsHeader, readingListData);
            return result;
        }
    }
}
