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
                FromPeriodCount = input.FromPeriodCount,
                ToPeriodCount = input.ToPeriodCount,
                input.ZoneIds,
            };
            IEnumerable<UnreadSummaryByZoneDataOutputDto> UnreadSummaryByZoneData = await _sqlReportConnection.QueryAsync<UnreadSummaryByZoneDataOutputDto>(UnreadSummaryByZone, @params);
            UnreadSummaryHeaderOutputDto UnreadSummaryByZoneHeader = new UnreadSummaryHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                FromPeriodCount = input.FromPeriodCount,
                ToPeriodCount = input.ToPeriodCount,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = UnreadSummaryByZoneData is not null && UnreadSummaryByZoneData.Any() ? UnreadSummaryByZoneData.Count() : 0,

                SumBarrier = UnreadSummaryByZoneData.Sum(u => u.Barrier),
                SumClosed = UnreadSummaryByZoneData.Sum(u => u.Closed),
            };

            var result = new ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto>(ReportLiterals.UnreadSummary + ReportLiterals.ByZone, UnreadSummaryByZoneHeader, UnreadSummaryByZoneData);
            return result;
        }

        private string GetUnreadSummaryByZoneQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @ZoneIds" : string.Empty;

            return @$";WITH Unread as
                       (Select 
                         	b.BillId AS BillId,
						    max(b.zoneId) AS ZoneId,
                         	max(b.ZoneTitle) AS ZoneTitle,
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
						MAX(t46.C2) AS RegionTitle,
                    	u.ZoneTitle ,
                    	SUM(Case When u.CounterStateId=7 then 1 else 0 end) AS Closed,
                    	SUM(Case When u.CounterStateId=4 then 1 else 0 end) AS Barrier
                    From Unread u
					Join [Db70].dbo.T51 t51
						On t51.C0=u.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Group By 
                    	u.ZoneTitle";
        }
    }
}
