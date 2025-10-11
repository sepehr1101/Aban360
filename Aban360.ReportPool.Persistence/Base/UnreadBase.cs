using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class UnreadBase : AbstractBaseConnection
    {
        public UnreadBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @ZoneIds" : string.Empty;

            return @$";WITH OrderedBills AS (
                    SELECT
                        TRIM(b.BillId) AS BillId,
                        b.CustomerNumber,
                        b.ReadingNumber,
                        b.CounterStateTitle,
                        b.ZoneTitle,
                        b.ZoneId,
                        b.TypeId,
                        b.SumItems,
                        b.RegisterDay,
                        ROW_NUMBER() OVER (PARTITION BY b.ZoneId, b.CustomerNumber ORDER BY b.RegisterDay DESC) AS RN
                    FROM [CustomerWarehouse].dbo.Bills b
                    WHERE 
                        (@FromReadingNumber IS NULL OR
                         @ToReadingNumber IS NULL OR
                         b.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                        AND b.ZoneId IN @zoneIds
                ),
                ConsecutiveCount AS (
                    SELECT 
                        ob.ZoneId,
                        ob.CustomerNumber,
                        SUM(CASE WHEN ob.TypeId = N'بسته مانع' THEN 1 ELSE 0 END) AS UnreadCount
                    FROM OrderedBills ob
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM OrderedBills x
                        WHERE 
                			x.CustomerNumber = ob.CustomerNumber AND
                            x.ZoneId = ob.ZoneId AND
                            x.RN < ob.RN AND
                            x.TypeId <> N'بسته مانع'
                    )
                    GROUP BY ob.ZoneId, ob.CustomerNumber
                ),
                LatestBill AS (
                    SELECT *
                    FROM OrderedBills
                    WHERE RN = 1
                )
                SELECT 
                    lb.BillId,
                    lb.CustomerNumber,
                    lb.ReadingNumber,
                    lb.CounterStateTitle,
                    lb.ZoneTitle,
                    lb.TypeId,
                    lb.SumItems,
                    c.FirstName + ' ' + c.Surename AS FullName,
                    c.FatherName,
                    c.WaterDiameterTitle AS MeterDiameterTitle,
                    c.UsageTitle AS UsageSellTitle,
                    c.Address,
                    c.WaterRequestDate AS MeterRequestDateJalali,
                    c.WaterRegisterDateJalali AS MeterInstallationDateJalali,
                    c.MobileNo AS MobileNumber,
                    c.PhoneNo AS PhoneNumber,
                    c.ContractCapacity AS ContractualCapacity,
                    c.CommercialCount AS CommercialUnit,
                    c.DomesticCount AS DomesticUnit,
                    c.OtherCount AS OtherUnit,
                    (c.ContractCapacity + c.DomesticCount + c.OtherCount) as TotalUnit,
                	c.UsageTitle,
                    c.MainSiphonTitle AS SiphonDiameterTitle,
                    c.EmptyCount AS EmptyUnit,
                    c.NationalId AS NationalCode,
                    ISNULL(cc.UnreadCount, 0) AS PeriodCount
                FROM LatestBill lb
                LEFT JOIN ConsecutiveCount cc
                    ON lb.CustomerNumber = cc.CustomerNumber AND lb.ZoneId = cc.ZoneId
                LEFT JOIN [CustomerWarehouse].dbo.Clients c
                    ON lb.ZoneId = c.ZoneId AND lb.CustomerNumber = c.CustomerNumber
                WHERE 
                	c.ToDayJalali IS NULL AND
                    ISNULL(cc.UnreadCount, 0) BETWEEN @FromPeriodCount AND @ToPeriodCount;";
        }
    
        internal string GetGroupedQuery(bool hasZone,string groupingField)
        {
            string zoneQuery = hasZone ? "AND b.ZoneId in @ZoneIds" : string.Empty;

            return $@";WITH OrderedBills AS (
                    SELECT
                        TRIM(b.BillId) AS BillId,
                        b.CustomerNumber,
                        b.ReadingNumber,
                        b.CounterStateTitle,
                        b.ZoneTitle,
                        b.ZoneId,
                		b.UsageTitle,
                        b.TypeId,
                        b.SumItems,
                        b.RegisterDay,
                        ROW_NUMBER() OVER (PARTITION BY b.ZoneId, b.CustomerNumber ORDER BY b.RegisterDay DESC) AS RN
                    FROM [CustomerWarehouse].dbo.Bills b
                    WHERE 
                        (@FromReadingNumber IS NULL OR
                         @ToReadingNumber IS NULL OR
                         b.ReadingNumber BETWEEN @FromReadingNumber AND @ToReadingNumber)
                         {zoneQuery}
                ),
                ConsecutiveCount AS (
                    SELECT 
                        ob.ZoneId,
                        ob.CustomerNumber,
                        SUM(CASE WHEN ob.TypeId = N'بسته مانع' THEN 1 ELSE 0 END) AS UnreadCount
                    FROM OrderedBills ob
                    WHERE NOT EXISTS (
                        SELECT 1
                        FROM OrderedBills x
                        WHERE 
                			x.CustomerNumber = ob.CustomerNumber AND
                            x.ZoneId = ob.ZoneId AND
                            x.RN < ob.RN AND
                            x.TypeId <> N'بسته مانع'
                    )
                    GROUP BY ob.ZoneId, ob.CustomerNumber
                ),
                LatestBill AS (
                    SELECT *
                    FROM OrderedBills
                    WHERE RN = 1
                ),
                Unread as(SELECT 
                    lb.BillId,
                	lb.UsageTitle,
                	lb.ZoneTitle,
                	c.DomesticCount,
                	c.CommercialCount,
                	c.OtherCount,
                	c.WaterDiameterId,
                	cc.UnreadCount
                FROM LatestBill lb
                LEFT JOIN ConsecutiveCount cc
                    ON lb.CustomerNumber = cc.CustomerNumber AND lb.ZoneId = cc.ZoneId
                LEFT JOIN [CustomerWarehouse].dbo.Clients c
                    ON lb.ZoneId = c.ZoneId AND lb.CustomerNumber = c.CustomerNumber
                WHERE 
                	c.ToDayJalali IS NULL AND
                    ISNULL(cc.UnreadCount, 0) BETWEEN @FromPeriodCount AND @ToPeriodCount
                )
                Select
                	u.{groupingField},
                	u.{groupingField} as ItemTitle,
                	COUNT(1) as CustomerCount,
                	COUNT(Case When u.UnreadCount=@FromPeriodCount Then 1 Else null End) as Count0,
                	COUNT(Case When u.UnreadCount=@FromPeriodCount+1 Then 1 Else null End) as Count1,
                	COUNT(Case When u.UnreadCount=@FromPeriodCount+2 Then 1 Else null End) as Count2,
                	COUNT(Case When u.UnreadCount>@FromPeriodCount+2 Then 1 Else null End) as CountMore,
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
                From Unread u
                Group By u.{groupingField}";
        }
    }
}
