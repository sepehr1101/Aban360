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
    internal sealed class ReadingListSummaryByUsageQueryService : AbstractBaseConnection, IReadingListSummaryByUsageQueryService
    {
        public ReadingListSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>> GetInfo(ReadingListInputDto input)
        {
            string modifiedBills = GetReadingListQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<ReadingListSummaryDataOutputDto> readingListData = await _sqlReportConnection.QueryAsync<ReadingListSummaryDataOutputDto>(modifiedBills, @params);
            ReadingListHeaderOutputDto modifiedBillsHeader = new ReadingListHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = readingListData is not null && readingListData.Any() ? readingListData.Count() : 0,
            };

            var result = new ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>(ReportLiterals.ReadingListSummary + ReportLiterals.ByUsage, modifiedBillsHeader, readingListData);
            return result;
        }

        private string GetReadingListQuery()
        {
            return @"Select
                    	b.UsageTitle AS ItemTitle,
                    	COUNT(1) AS ReadingCount,
                    	COUNT(Case When b.CounterStateCode=4 Then 1 ENd) AS CloseCount,
                    	COUNT(Case When b.CounterStateCode=7 Then 1 End) AS ObstacleCount,
                    	COUNT(Case When b.CounterStateCode=2 Then 1 ENd) AS ReplacementBranchCount,
                    	COUNT(Case When b.CounterStateCode=1 Then 1 ENd) AS MalfunctionCount,
						COUNT(Case When b.CounterStateCode NOT IN (4,7,8) Then 1 End) AS NetCount
                    	COUNT(Case When b.CounterStateCode=8 Then 1 End) AS AdvancePaymentCount,
						COUNT(Case When b.ReadingStateTitle IN (N'خوداظهاری حضوری',N'خوداظهاری غیرحضوری')Then 1 End) as SelfClaimedCount
                    From [CustomerWarehouse].dbo.Bills b
                    Where
                    	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	b.NextDay BETWEEN @fromDate AND @toDate AND
                        b.ZoneId IN @zoneIds
                    Group By B.UsageTitle";
        }
    }
}
