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
    internal sealed class UnreadSummaryByUsageQueryService : AbstractBaseConnection, IUnreadSummaryByUsageQueryService
    {
        public UnreadSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string UnreadSummaryByUsage = GetUnreadSummaryByUsageQuery(input.ZoneIds?.Any() == true);
            var @params = new
            {
                input.FromReadingNumber,
                input.ToReadingNumber,
                FromPeriodCount = input.FromPeriodCount,
                ToPeriodCount = input.ToPeriodCount,
                input.ZoneIds,
            };
            IEnumerable<UnreadSummaryDataOutputDto> UnreadSummaryByUsageData = await _sqlReportConnection.QueryAsync<UnreadSummaryDataOutputDto>(UnreadSummaryByUsage,
                                                                                                                                                 @params);
            UnreadSummaryHeaderOutputDto UnreadSummaryByUsageHeader = new UnreadSummaryHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromPeriodCount = input.FromPeriodCount,
                ToPeriodCount = input.ToPeriodCount,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = UnreadSummaryByUsageData is not null && UnreadSummaryByUsageData.Any() ? UnreadSummaryByUsageData.Count() : 0,

                SumBarrier = UnreadSummaryByUsageData.Sum(u => u.Barrier),
                SumClosed = UnreadSummaryByUsageData.Sum(u => u.Closed),
            };

            var result = new ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryDataOutputDto>(ReportLiterals.UnreadSummary + ReportLiterals.ByUsage, UnreadSummaryByUsageHeader, UnreadSummaryByUsageData);
            return result;
        }

        private string GetUnreadSummaryByUsageQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @ZoneIds" : string.Empty;

            return $@";WITH Unread as
                       (Select 
                       	b.BillId AS BillId,
                       	max(b.UsageTitle) AS UsageTitle,
					     	max(b.CounterStateCode) AS CounterStateId
                       From [CustomerWarehouse].dbo.Bills b
                       JOIN [CustomerWarehouse].dbo.Clients c ON b.BillId=c.BillId
                       LEFT JOIN [CustomerWarehouse].dbo.payments as p ON p.BillTableId = b.id
                       WHERE
            		     	c.ToDayJalali IS NULL AND
                       	p.id IS NULL AND
                           b.TypeId=N'بسته مانع' AND
                        (@FromReadingNumber IS NULL OR
                         	@ToReadingNumber IS NULL OR
                         	c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                           {zoneQuery}
                       GROUP BY b.BillId
                       HAVING COUNT(b.BillId) BETWEEN @FromPeriodCount AND @ToPeriodCount)
                    Select
                    	u.UsageTitle AS ItemTitle,
                    	SUM(Case When u.CounterStateId=7 then 1 else 0 end) AS Closed,
                    	SUM(Case When u.CounterStateId=4 then 1 else 0 end) AS Barrier
                    From Unread u
                    Group By 
                    	u.UsageTitle";
        }
    }
}
