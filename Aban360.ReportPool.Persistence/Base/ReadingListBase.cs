using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class ReadingListBase : AbstractBaseConnection
    {
        public ReadingListBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery()
        {
            return @"";
        }

        internal string GetGroupedQuery(string groupingField)
        {
            return $@"Select
						MAX(t46.C2) AS RegionTitle,
                    	b.{groupingField} as ItemTitle,
                    	b.{groupingField},
                    	COUNT(1) AS ReadingCount,
                    	COUNT(Case When b.CounterStateCode=4 Then 1 ENd) AS CloseCount,
                    	COUNT(Case When b.CounterStateCode=7 Then 1 End) AS ObstacleCount,
                    	COUNT(Case When b.CounterStateCode=2 Then 1 ENd) AS ReplacementBranchCount,
                    	COUNT(Case When b.CounterStateCode=1 Then 1 ENd) AS MalfunctionCount,
						COUNT(Case When b.CounterStateCode NOT IN (4,7,8) Then 1 End) AS NetCount,
                    	COUNT(Case When b.CounterStateCode=8 Then 1 End) AS AdvancePaymentCount,
						COUNT(Case When b.ReadingStateTitle IN (N'خوداظهاری حضوری',N'خوداظهاری غیرحضوری')Then 1 End) as SelfClaimedCount
                    From [CustomerWarehouse].dbo.Bills b
					Join [Db70].dbo.T51 t51
						On t51.C0=b.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where
                        (@FromReadingNumber IS NULL or
                    	@ToReadingNumber IS NULL or 
                    	b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	b.NextDay BETWEEN @fromDate AND @toDate AND
                        b.ZoneId IN @zoneIds
                    Group By B.{groupingField}";
        }
    }
}
