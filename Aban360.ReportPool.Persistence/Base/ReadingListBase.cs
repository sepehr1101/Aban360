using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class ReadingListBase : AbstractBaseConnection
    {
        public ReadingListBase(IConfiguration configuration)
            : base(configuration)
        {
        }

        internal string GetDetailQuery(bool hasUsage)
        {
            string usageQuery = hasUsage ? @"AND b.UsageId IN @usageIds" : string.Empty;

            return @$"Select
						t46.C2 AS RegionTitle,
                    	b.ZoneTitle,
						b.CustomerNumber ,
						b.ReadingNumber ,
						b.BillId,
						c.FirstName ,
						c.SureName as Surname,
						(c.FirstName + ' ' + c.SureName) as FullName,
                        c.ContractCapacity AS ContractualCapacity,
                        c.CommercialCount AS CommercialUnit,
                        c.DomesticCount AS DomesticUnit,
                        c.OtherCount AS OtherUnit,
                        IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
						b.Consumption,
						b.ConsumptionAverage,
						b.CounterStateTitle,
						b.CounterStateCode as CounterStateId,
						b.SumItems,
						b.WaterDiameterId as MeterDiameterId,
						b.WaterDiameterTitle as MeterDiameterTitle,
						c.MainSiphonTitle as SiphonDiameterTitle,
						b.IsSettlement as IsSelfClaimed
                    From [CustomerWarehouse].dbo.Bills b
					Left Join [CustomerWarehouse].dbo.Clients c	
						ON b.ZoneId=c.ZoneId AND b.CustomerNumber=c.CustomerNumber
					Join [Db70].dbo.T51 t51
						On t51.C0=b.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where
						c.ToDayJalali IS NULL AND
                        (@FromReadingNumber IS NULL or
                    	@ToReadingNumber IS NULL or 
                    	b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	b.NextDay BETWEEN @FromDateJalali AND @ToDateJalali AND
                        b.ZoneId IN @zoneIds 
                        {usageQuery}";
        }

        internal string GetGroupedQuery(string groupingField)
        {
            return $@"Select
						MAX(t46.C2) AS RegionTitle,
                    	b.{groupingField} as ItemTitle,
                    	b.{groupingField},
                    	COUNT(1) AS ReadingCount,
                    	COUNT(Case When b.CounterStateCode=4 Then 1 END) AS CloseCount,
                    	COUNT(Case When b.CounterStateCode=7 Then 1 EnD) AS ObstacleCount,
                    	COUNT(Case When b.CounterStateCode=2 Then 1 END) AS ReplacementBranchCount,
                    	COUNT(Case When b.CounterStateCode=1 Then 1 END) AS MalfunctionCount,
						COUNT(Case When b.CounterStateCode NOT IN (4,7,8) Then 1 End) AS PureCount,
                    	COUNT(Case When b.CounterStateCode=8 Then 1 End) AS AdvancePaymentCount,
						COUNT(Case When b.IsSettlement=1 Then 1 End) as SelfClaimedCount
                    From [CustomerWarehouse].dbo.Bills b
					Join [Db70].dbo.T51 t51
						On t51.C0=b.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where
                        (@FromReadingNumber IS NULL or
                    	@ToReadingNumber IS NULL or 
                    	b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	b.NextDay BETWEEN @FromDateJalali AND @ToDateJalali AND
                        b.ZoneId IN @zoneIds
                    Group By B.{groupingField}";
        }
    }
}
