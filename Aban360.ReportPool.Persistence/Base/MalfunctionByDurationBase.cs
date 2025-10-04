using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class MalfunctionByDurationBase: AbstractBaseConnection
    {
        protected MalfunctionByDurationBase(IConfiguration configuration)
            :base(configuration)
        {            
        }
        internal string GetDetailsQueryLatest()
        {
            return @"-- آخرین قبض معتبر
                    ;WITH ValidLatestBills AS (
                        SELECT
                            b.BillId,
                            b.CustomerNumber,
                            b.ZoneId,
                            b.ZoneTitle,
                            b.ReadingNumber,
                            b.UsageTitle,
                            b.ConsumptionAverage,
                            b.Consumption,
	                        b.SumItems,
                            b.CounterStateCode,
                            b.RegisterDay AS LatestRegisterDay,
                            b.ContractCapacity AS ContractualCapacity
                        FROM (
                            SELECT *,
                                   ROW_NUMBER() OVER (PARTITION BY BillId ORDER BY RegisterDay DESC) AS rn
                            FROM [CustomerWarehouse].dbo.Bills
                            WHERE 
                    			ZoneId IN @zoneIds AND 
                    			CounterStateCode NOT IN (4,7,8)
                        ) b
                        WHERE 
                    		b.rn = 1 AND 
                    		b.CounterStateCode = 1 AND 
                    		NOT EXISTS (--بعد از اخرین قرائت تعویض شده اند
                              SELECT 1
                              FROM [CustomerWarehouse].dbo.MeterChange mc
                              WHERE 
                    			mc.CustomerNumber = b.CustomerNumber
                                AND mc.ZoneId = b.ZoneId
                                AND mc.ChangeDateJalali >= b.NextDay
                          )
                    ),
                    -- محاسبه تعداد دوره‌های خرابی
                    FinalCount AS (
                        SELECT 
                            b.BillId,
                            COUNT(1) AS MalfunctionPeriodCount
                        FROM [CustomerWarehouse].dbo.Bills b
                        INNER JOIN ValidLatestBills v 
                    		ON v.CustomerNumber = b.CustomerNumber AND v.ZoneId=b.ZoneId
                        WHERE 
                    	  b.CounterStateCode = 1 AND 
                    	  b.RegisterDay <= v.LatestRegisterDay 
                        GROUP BY b.BillId
                    )
                    SELECT 
                        v.BillId,
                        v.CustomerNumber,
                        v.ZoneId,
                        v.ZoneTitle,
                        v.ReadingNumber,
                        v.UsageTitle,
                        v.ConsumptionAverage,
                        v.CounterStateCode,
                        v.ContractualCapacity,
                        v.LatestRegisterDay,
                        f.MalfunctionPeriodCount,
                        ISNULL(lc.ChangeDateJalali, 0) AS LastChangeDateJalali,
						c.BillId,
                        TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) AS Surname,
                        c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.DomesticCount AS DomesticUnit,
                        c.CommercialCount AS CommercialUnit,
                        c.OtherCount AS OtherUnit,
                        TRIM(c.Address) AS Address,
						c.BranchType,
						c.MainSiphonTitle AS SiphonDiameterTitle,
                        TRIM(c.PhoneNo) AS PhoneNumber,
                        TRIM(c.MobileNo) AS MobileNumber,
                        c.WaterInstallDate AS MeterInstallationDateJalali,
                        c.WaterRequestDate AS MeterRequestDateJalali,
                        c.DeletionStateTitle,
                        v.Consumption,
	                    v.SumItems
                    FROM ValidLatestBills v
                    INNER JOIN FinalCount f 
                    	ON v.BillId = f.BillId AND (f.MalfunctionPeriodCount BETWEEN @fromMalfunctionPeriodCount AND @toMalfunctionPeriodCount)
					INNER JOIN [CustomerWarehouse].dbo.Clients c
						ON v.BillId=c.BillId
                    OUTER APPLY (
                        SELECT TOP 1 mc.ChangeDateJalali
                        FROM [CustomerWarehouse].dbo.MeterChange mc
                        WHERE 
                    		mc.CustomerNumber = v.CustomerNumber AND 
                            mc.ZoneId = v.ZoneId 
                        ORDER BY mc.ChangeDateJalali DESC
                    ) lc
					Where 
                    	c.ToDayJalali IS NULL AND
                        (@fromReadingNumber IS NULL OR
                            @toReadingNumber IS NULL OR  
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber )AND
                        c.ZoneId IN @zoneIds AND
						c.DeletionStateId NOT IN (1,2,5)
                    ORDER BY lc.ChangeDateJalali DESC;";
        }
        internal string GetDetailsQuery()
        {
            return @"-- آخرین قبض معتبر
                    ;WITH ValidLatestBills AS (
                        SELECT
                            b.BillId,
							COUNT(b.BillId) as MalfunctionPeriodCount,
                            AVG(b.ConsumptionAverage)as ConsumptionAverage,
                            SUM(b.Consumption)as Consumption,
	                        SUM(b.SumItems)as SumItems,
                            MAX(b.RegisterDay) AS LatestRegisterDay
                        FROM [CustomerWarehouse].dbo.Bills b
                        WHERE                     		
                    		b.CounterStateCode = 1 AND 
                    		NOT EXISTS (--بعد از اخرین قبض تعویض شده اند
                              SELECT 1
                              FROM [CustomerWarehouse].dbo.MeterChange mc
                              WHERE 
                    			mc.CustomerNumber = b.CustomerNumber
                                AND mc.ZoneId = b.ZoneId
                                AND mc.ChangeDateJalali >= b.NextDay
                          )
						  Group By b.BillId
						  Having COUNT(b.BillId) BETWEEN @fromMalfunctionPeriodCount AND @toMalfunctionPeriodCount

                    )
                    SELECT 
                        v.ConsumptionAverage,
                        v.LatestRegisterDay,
                        v.MalfunctionPeriodCount,
                        ISNULL(lc.ChangeDateJalali, 0) AS LastChangeDateJalali,
						c.BillId,
                        c.CustomerNumber,
                        c.ZoneId,
                        c.ZoneTitle,
                        c.ReadingNumber,
                        c.UsageTitle,
						c.ContractCapacity AS ContractualCapacity,
                        TRIM(c.FirstName)+' '+TRIM(c.SureName) AS FullName,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) AS Surname,
                        c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.DomesticCount AS DomesticUnit,
                        c.CommercialCount AS CommercialUnit,
                        c.OtherCount AS OtherUnit,
                        TRIM(c.Address) AS Address,
						c.BranchType,
						c.MainSiphonTitle AS SiphonDiameterTitle,
                        TRIM(c.PhoneNo) AS PhoneNumber,
                        TRIM(c.MobileNo) AS MobileNumber,
                        c.WaterInstallDate AS MeterInstallationDateJalali,
                        c.WaterRequestDate AS MeterRequestDateJalali,
                        c.DeletionStateTitle,
                        v.Consumption,
	                    v.SumItems
                    FROM ValidLatestBills v
                      INNER JOIN [CustomerWarehouse].dbo.Clients c 
                    	ON v.BillId = c.BillId
                    OUTER APPLY (
                        SELECT TOP 1 mc.ChangeDateJalali
                        FROM [CustomerWarehouse].dbo.MeterChange mc
                        WHERE 
                    		mc.CustomerNumber = c.CustomerNumber AND 
                            mc.ZoneId = c.ZoneId
                        ORDER BY mc.ChangeDateJalali DESC
                    ) lc
					 WHERE 
                    	c.ToDayJalali IS NULL AND
                        (@fromReadingNumber IS NULL OR
                         @toReadingNumber IS NULL OR  
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber )AND
                        c.ZoneId IN @zoneIds AND
						c.DeletionStateId NOT IN (1,2,5)
                    ORDER BY lc.ChangeDateJalali DESC;";
        }
        internal string GetGroupedQuery(bool isZone)
        {
            string parameter = GetQueryParam(isZone);
            return @$"-- آخرین قبض معتبر
                    ;WITH ValidLatestBills AS (
                        SELECT
                            b.BillId,
							COUNT(b.BillId) as MalfunctionPeriodCount,
                            AVG(b.ConsumptionAverage)as ConsumptionAverage,
                            SUM(b.Consumption)as Consumption,
	                        SUM(b.SumItems)as SumItems,
                            MAX(b.RegisterDay) AS LatestRegisterDay
                        FROM [CustomerWarehouse].dbo.Bills b
                        WHERE                     		
                    		b.CounterStateCode = 1 AND 
                    		NOT EXISTS (--بعد از اخرین قبض تعویض شده اند
                              SELECT 1
                              FROM [CustomerWarehouse].dbo.MeterChange mc
                              WHERE 
                    			mc.CustomerNumber = b.CustomerNumber
                                AND mc.ZoneId = b.ZoneId
                                AND mc.ChangeDateJalali >= b.NextDay
                          )
						  Group By b.BillId
						  Having COUNT(b.BillId) BETWEEN @fromMalfunctionPeriodCount AND @toMalfunctionPeriodCount
                    )
                    SELECT 
				    		MAX(t46.C2) AS RegionTitle,
                            c.{parameter} AS ItemTitle,
                        	COUNT(c.{parameter}) AS CustomerCount,
				    		SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
				    		SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
				    		SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
				    		SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
				    		SUM(CASE WHEN c.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
				    		SUM(CASE WHEN c.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
				    		SUM(CASE WHEN c.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
				    		SUM(CASE WHEN c.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
				    		SUM(CASE WHEN c.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
				    		SUM(CASE WHEN c.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
				    		SUM(CASE WHEN c.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
				    		SUM(CASE WHEN c.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
				    		SUM(CASE WHEN c.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
				    		SUM(CASE WHEN c.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
				    		SUM(CASE WHEN c.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    FROM ValidLatestBills v
					INNER JOIN [CustomerWarehouse].dbo.Clients c
                    	ON v.BillId = c.BillId
				    Join [Db70].dbo.T51 t51
				    	On t51.C0=c.ZoneId
				    Join [Db70].dbo.T46 t46
				    	On t51.C1=t46.C0
                    OUTER APPLY (
                        SELECT TOP 1 mc.ChangeDateJalali
                        FROM [CustomerWarehouse].dbo.MeterChange mc
                        WHERE 
                    		mc.CustomerNumber = c.CustomerNumber AND 
                            mc.ZoneId = c.ZoneId
                        ORDER BY mc.ChangeDateJalali DESC
                    ) lc
					 WHERE 
                    	c.ToDayJalali IS NULL AND
                        (@fromReadingNumber IS NULL OR
                         @toReadingNumber IS NULL OR  
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber )AND
                        c.ZoneId IN @zoneIds AND
						c.DeletionStateId NOT IN (1,2,5)
					GROUP BY c.{parameter}";
        }
        internal string GetGroupedQueryLatest(bool isZone)
        {
            string parameter = GetQueryParam(isZone);
            return $@"-- آخرین قبض معتبر
                    ;WITH ValidLatestBills AS (
                        SELECT
                            b.BillId,
				    		b.CustomerNumber,
                            b.ZoneId,
                            b.UsageTitle,
                            b.ZoneTitle,
                            b.CounterStateCode,
                            b.RegisterDay AS LatestRegisterDay
                        FROM (
                            SELECT *,
                                   ROW_NUMBER() OVER (PARTITION BY BillId ORDER BY RegisterDay DESC) AS rn
                            FROM [CustomerWarehouse].dbo.Bills
                            WHERE 
                       			ZoneId IN @zoneIds AND 
                       			CounterStateCode NOT IN (4,7,8) 
                        ) b
                        WHERE 
                       		b.rn = 1 AND 
                       		b.CounterStateCode = 1 AND 
                       		NOT EXISTS (--بعد از اخرین قبض تعویض شده اند
                              SELECT 1
                              FROM [CustomerWarehouse].dbo.MeterChange mc
                              WHERE 
                       			mc.CustomerNumber = b.CustomerNumber
                                AND mc.ZoneId = b.ZoneId
                                AND mc.ChangeDateJalali >= b.NextDay
                          )
                    ),
                    -- محاسبه تعداد دوره‌های خرابی
                    FinalCount AS (
                        SELECT 
                            b.BillId,
                            COUNT(1) AS MalfunctionPeriodCount
                        FROM [CustomerWarehouse].dbo.Bills b
                        INNER JOIN ValidLatestBills v 
                       		ON v.CustomerNumber = b.CustomerNumber AND v.ZoneId=b.ZoneId
                        WHERE 
                       	  b.CounterStateCode = 1 AND 
                       	  b.RegisterDay <= v.LatestRegisterDay
                        GROUP BY b.BillId
                    )
                    SELECT 
				    	MAX(t46.C2) AS RegionTitle,
                        v.{parameter}  AS ItemTitle,
                        COUNT(c.{parameter}) AS CustomerCount,
				    	SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
				    	SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
				    	SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
				    	SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
				    	SUM(CASE WHEN c.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
				    	SUM(CASE WHEN c.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
				    	SUM(CASE WHEN c.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
				    	SUM(CASE WHEN c.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
				    	SUM(CASE WHEN c.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
				    	SUM(CASE WHEN c.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
				    	SUM(CASE WHEN c.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
				    	SUM(CASE WHEN c.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
				    	SUM(CASE WHEN c.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
				    	SUM(CASE WHEN c.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
				    	SUM(CASE WHEN c.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    FROM ValidLatestBills v
                    INNER JOIN FinalCount f 
                       	ON v.BillId = f.BillId AND (f.MalfunctionPeriodCount BETWEEN @fromMalfunctionPeriodCount AND @toMalfunctionPeriodCount)
				    INNER JOIN [CustomerWarehouse].dbo.Clients c
				    	On v.BillId=c.BillId
				    Join [Db70].dbo.T51 t51
				    	On t51.C0=c.ZoneId
				    Join [Db70].dbo.T46 t46
				    	On t51.C1=t46.C0
                    WHERE 
                    	c.ToDayJalali IS NULL AND
                        (@fromReadingNumber IS NULL OR
                            @toReadingNumber IS NULL OR  
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber )AND
                        c.ZoneId IN @zoneIds AND
						c.DeletionStateId NOT IN (1,2,5)
				    Group by v.{parameter}";
        }
    
        private string GetQueryParam(bool isZone)
        {
            string ZoneTitle = nameof(ZoneTitle),
                   UsageTitle = nameof(UsageTitle);

            return isZone?ZoneTitle:UsageTitle;
        }
    }
}
