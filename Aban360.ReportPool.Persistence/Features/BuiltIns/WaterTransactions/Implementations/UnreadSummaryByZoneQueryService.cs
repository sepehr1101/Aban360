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
    internal sealed class UnreadSummaryByZoneQueryService : UnreadBase, IUnreadSummaryByZoneQueryService
    {
        public UnreadSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto>> GetInfo(UnreadInputDto input)
        {
            string query = GetGroupedQuery(input.ZoneIds?.Any() == true,GroupingFields.ZoneTitle);
            //string query = GetUnreadSummaryByZoneQuery(input.ZoneIds?.Any() == true);
            
            var @params = new
            {
                input.FromReadingNumber,
                input.ToReadingNumber,
                FromPeriodCount = input.FromPeriodCount,
                ToPeriodCount = input.ToPeriodCount,
                input.ZoneIds,
            };
            IEnumerable<UnreadSummaryByZoneDataOutputDto> UnreadSummaryByZoneData = await _sqlReportConnection.QueryAsync<UnreadSummaryByZoneDataOutputDto>(query, @params, null, 180);
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
                SumCommercialUnit = UnreadSummaryByZoneData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = UnreadSummaryByZoneData.Sum(i => i.DomesticUnit),
                SumOtherUnit = UnreadSummaryByZoneData.Sum(i => i.OtherUnit),
                TotalUnit = UnreadSummaryByZoneData.Sum(i => i.TotalUnit),
                CustomerCount = UnreadSummaryByZoneData.Sum(i => i.CustomerCount),
                Count0 = UnreadSummaryByZoneData?.Sum(i => i.Count0) ?? 0,
                Count1 = UnreadSummaryByZoneData?.Sum(i => i.Count1) ?? 0,
                Count2 = UnreadSummaryByZoneData?.Sum(i => i.Count2) ?? 0,
                CountMore = UnreadSummaryByZoneData?.Sum(i => i.CountMore) ?? 0,
            };

            var result = new ReportOutput<UnreadSummaryHeaderOutputDto, UnreadSummaryByZoneDataOutputDto>(ReportLiterals.UnreadSummary + ReportLiterals.ByZone, UnreadSummaryByZoneHeader, UnreadSummaryByZoneData);
            return result;
        }

        private string GetUnreadSummaryByZoneQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @ZoneIds" : string.Empty;

            return @$";WITH LatestBill AS (
                    SELECT
                        TRIM(b.BillId) AS BillId,
                		b.CustomerNumber,
                		b.ReadingNumber,
                		b.CounterStateTitle,
						b.CounterStateCode,
                		b.ZoneTitle,
                		b.ZoneId,
						b.UsageTitle,
                        b.TypeId,
                		b.SumItems,
                        ROW_NUMBER() OVER (PARTITION BY TRIM(b.BillId) ORDER BY b.RegisterDay DESC) AS RN
                    FROM [CustomerWarehouse].dbo.Bills b
                    WHERE 
                        (@FromReadingNumber IS NULL OR
                         @ToReadingNumber IS NULL OR
                         b.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                        {zoneQuery}
                ),
			UnreadBill as(
                    SELECT 
                		l.BillId,
						MAX(l.UsageTitle)as UsageTitle,
						MAX(l.ZoneTitle)as ZoneTitle,
						MAX(l.CounterStateCode)as CounterStateCode,
						MAX(c.WaterDiameterId) as WaterDiameterId,
						MAX(c.CommercialCount) as CommercialCount,
						MAX(c.DomesticCount) as DomesticCount,
						MAX(c.OtherCount) as OtherCount
                    FROM LatestBill l
                	Left Join [CustomerWarehouse].dbo.Clients c
                		On l.ZoneId=c.ZoneId AND l.CustomerNumber=c.CustomerNumber
                    WHERE
                		(l.RN BETWEEN @FromPeriodCount AND @ToPeriodCount) AND 
                		c.ToDayJalali IS NULL
                    GROUP BY l.BillId
                    HAVING COUNT(CASE WHEN l.TypeId = N'بسته مانع' THEN 1 END) >= @ToPeriodCount
					)
					Select
						u.ZoneTitle,
						u.ZoneTitle AS ItemTitle,
						SUM(Case When u.CounterStateCode=7 then 1 else 0 end) AS Closed,
						SUM(Case When u.CounterStateCode=4 then 1 else 0 end) AS Barrier,
						COUNT(1) AS CustomerCount,
						SUM(ISNULL(u.CommercialCount, 0) + ISNULL(u.DomesticCount, 0) + ISNULL(u.OtherCount, 0)) AS TotalUnit,
						SUM(ISNULL(u.CommercialCount, 0)) AS CommercialUnit,
						SUM(ISNULL(u.DomesticCount, 0)) AS DomesticUnit,
						SUM(ISNULL(u.OtherCount, 0)) AS OtherUnit,
						SUM(CASE WHEN u.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						SUM(CASE WHEN u.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
						SUM(CASE WHEN u.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
						SUM(CASE WHEN u.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
						SUM(CASE WHEN u.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
						SUM(CASE WHEN u.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
						SUM(CASE WHEN u.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
						SUM(CASE WHEN u.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
						SUM(CASE WHEN u.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
						SUM(CASE WHEN u.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
						SUM(CASE WHEN u.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
					From UnreadBill u
					Group By u.ZoneTitle";
        }
    }
}
