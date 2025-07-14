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
    internal sealed class ReadingListSummaryQueryService : AbstractBaseConnection, IReadingListSummaryQueryService
    {
        public ReadingListSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>> GetInfo(ReadingListInputDto input)
        {
            string modifiedBills = GetReadingListQuery();
            var @params = new
            {
                fromReadingNumber=input.FromReadingNumber,
                toReadingNumber=input.ToReadingNumber,
                fromDate=input.FromDateJalali,
                toDate=input.ToDateJalali,
                zoneIds=input.ZoneIds,
            };
            IEnumerable<ReadingListSummaryDataOutputDto> readingListData = await _sqlReportConnection.QueryAsync<ReadingListSummaryDataOutputDto>(modifiedBills, @params);
            ReadingListHeaderOutputDto modifiedBillsHeader = new ReadingListHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = readingListData.Count(),
            };

            var result = new ReportOutput<ReadingListHeaderOutputDto, ReadingListSummaryDataOutputDto>(ReportLiterals.ReadingListSummary, modifiedBillsHeader, readingListData);
            return result;
        }

        private string GetReadingListQuery()
        {
            return @"Select
                    	b.ZoneTitle,
                    	COUNT(1) AS ReadingCount,
                    	COUNT(Case When b.CounterStateCode=4 Then 1 ENd) AS CloseCount,
                    	COUNT(Case When b.CounterStateCode=7 Then 1 End) AS ObstacleCount,
                    	COUNT(Case When b.CounterStateCode=2 Then 1 ENd) AS ReplacementBranchCount,
                    	COUNT(Case When b.CounterStateCode=8 Then 1 End) AS AdvancePaymentCount
                    From [CustomerWarehouse].dbo.Bills b
                    Where
                    	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                        b.ZoneId IN @zoneIds
                    Group By B.ZoneTitle";
        }

    }
}
