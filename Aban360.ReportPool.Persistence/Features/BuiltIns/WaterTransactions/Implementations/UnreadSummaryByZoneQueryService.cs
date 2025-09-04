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
    internal sealed class UnreadSummaryByZoneQueryService : AbstractBaseConnection, IUnreadSummaryByZoneQueryService
    {
        public UnreadSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string UnreadSummaryByZone = GetUnreadSummaryByZoneQuery(input.ZoneIds?.Any() == true);
            var @params = new
            {
                input.FromReadingNumber,
                input.ToReadingNumber,
                input.PeriodCount,
                input.ZoneIds,
            };
            IEnumerable<UnreadSummaryByZoneDataOutputDto> UnreadSummaryByZoneData = await _sqlReportConnection.QueryAsync<UnreadSummaryByZoneDataOutputDto>(UnreadSummaryByZone, @params);
            UnreadSummaryHeaderOutputDto UnreadSummaryByZoneHeader = new UnreadSummaryHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                PeriodCount = input.PeriodCount,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = UnreadSummaryByZoneData is not null && UnreadSummaryByZoneData.Any() ? UnreadSummaryByZoneData.Count() : 0,

                SumBarrier = UnreadSummaryByZoneData.Sum(u => u.Barrier),
                SumClosed = UnreadSummaryByZoneData.Sum(u => u.Closed),
            };

            var result = new ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto>(ReportLiterals.Unread + ReportLiterals.ByZone, UnreadSummaryByZoneHeader, UnreadSummaryByZoneData);
            return result;
        }

        private string GetUnreadSummaryByZoneQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @ZoneIds" : string.Empty;

            return @$";WITH Unread as
                       (Select 
                       	b.BillId AS BillId,
                       	max(b.ZoneTitle) AS ZoneTitle,
					     	max(b.CounterStateCode) AS CounterStateId
                       From [CustomerWarehouse].dbo.Bills b
                       JOIN [CustomerWarehouse].dbo.Clients c ON b.BillId=c.BillId
                       LEFT JOIN [CustomerWarehouse].dbo.payments as p ON p.BillTableId = b.id
                       WHERE
            		     	c.ToDayJalali IS NULL AND
                       	p.id IS NULL AND
                           b.TypeId=N'بسته مانع' AND
                            (
                            	(@FromReadingNumber IS NOT NULL AND
                            		@ToReadingNumber IS NOT NULL AND
                            		c.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                            	OR
                            	(@FromReadingNumber IS NULL AND
                            		@ToReadingNumber IS NULL)
                            )
                           {zoneQuery}
                       GROUP BY b.BillId
                       HAVING COUNT(b.BillId)>=0)
                    Select
                    	u.ZoneTitle,
                    	Count(Case When u.CounterStateId=7 then 1 else 0 end) AS Closed,
                    	Count(Case When u.CounterStateId=4 then 1 else 0 end) AS Barrier
                    From Unread u
                    Group By 
                    	u.ZoneTitle";
        }
    }
}
