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
                SumCommercialUnit = UnreadSummaryByUsageData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = UnreadSummaryByUsageData.Sum(i => i.DomesticUnit),
                SumOtherUnit = UnreadSummaryByUsageData.Sum(i => i.OtherUnit),
                TotalUnit = UnreadSummaryByUsageData.Sum(i => i.TotalUnit),
                CustomerCount = UnreadSummaryByUsageData.Sum(i => i.CustomerCount),
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
					     	max(b.CounterStateCode) AS CounterStateId,
							MAX(b.DomesticCount) as DomesticCount, 
							MAX(b.CommercialCount) as CommercialCount,
							MAX(b.OtherCount) as OtherCount,
							MAX(b.WaterDiameterId) as WaterDiameterId 
                       From [CustomerWarehouse].dbo.Bills b
                       JOIN [CustomerWarehouse].dbo.Clients c 
                            ON b.BillId=c.BillId
                       LEFT JOIN [CustomerWarehouse].dbo.payments as p 
                            ON p.BillTableId = b.id
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
                    	SUM(Case When u.CounterStateId=4 then 1 else 0 end) AS Barrier,
                    	COUNT(u.UsageTitle) AS CustomerCount,
					    SUM(ISNULL(u.CommercialCount, 0) + ISNULL(u.DomesticCount, 0) + ISNULL(u.OtherCount, 0)) AS TotalUnit,
                        SUM(ISNULL(u.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(u.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(u.OtherCount, 0)) AS OtherUnit,
						SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
				        SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
				        SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
				        SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
				        SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
				        SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
				        SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
				        SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
				        SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
				        SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
				        SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From Unread u
					Join [Db70].dbo.T5 t5
					 	On t5.C0=u.WaterDiameterId
                    Group By 
                    	u.UsageTitle";
        }
    }
}
