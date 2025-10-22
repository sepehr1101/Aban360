using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class MalfunctionMeterBase : AbstractBaseConnection
    {
        public MalfunctionMeterBase(IConfiguration configuration)
            : base(configuration)
        { 
		}

        internal string GetDetailQuery()
        {
            return @";WITH CTE AS (
                    Select
						TRIM(c.FirstName)+TRIM(c.SureName) As FullName,
                        b.BillId,
                        b.ZoneTitle,
                        b.CustomerNumber,
                        b.ReadingNumber,
                        b.Duration,
                        b.BranchType,
                        b.RegisterDay AS LastReadingDay,
                        b.Payable,
	                    b.SumItems,
                        b.Consumption,
                        b.ConsumptionAverage,
	                    b.CounterStateCode,
	                    b.CounterStateTitle,
						c.WaterInstallDate	AS MeterInstallationDateJalali,
						m.ChangeDateJalali AS LatestChangeDateJalali,
	                    RN=ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay,m.ChangeDateJalali DESC)
                    From [CustomerWarehouse].dbo.Bills b
					Left Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
					Left Join [CustomerWarehouse].dbo.MeterChange m
						On c.ZoneId=m.ZoneId AND c.CustomerNumber=m.CustomerNumber
                    Where 
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        b.ZoneId in @zoneIds AND
                        b.CounterStateCode NOT IN (4,7,8) AND
						c.DeletionStateId IN (0,1) AND
						c.ToDayJalali IS NULL AND
						(@FromDateJalali IS NULL OR
						@ToDateJalali IS NULL OR
						b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali)
						)
                    SELECT * FROM CTE 
                    WHERE RN=1 AND 
					CounterStateCode=1";
        }

        //TODO: انشعابات جمع آوری شده جهت تطبیق گزارش ها موقتا نادیده گرفته شده
        internal string GetDetailsNonDuplicate()
		{
            return @";WITH CTE AS (
                    Select
						TRIM(c.FirstName)+TRIM(c.SureName) As FullName,
                        b.BillId,
                        b.ZoneTitle,
                        b.CustomerNumber,
                        b.ReadingNumber,
                        b.Duration,
                        b.BranchType,
                        b.RegisterDay AS LastReadingDay,
                        b.Payable,
	                    b.SumItems,
                        b.Consumption,
                        b.ConsumptionAverage,
	                    b.CounterStateCode,
	                    b.CounterStateTitle,
						c.WaterInstallDate	AS MeterInstallationDateJalali,
						m.ChangeDateJalali AS LatestChangeDateJalali,
	                    RN=ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay,m.ChangeDateJalali DESC)
                    From [CustomerWarehouse].dbo.Bills b
					Left Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
					Left Join [CustomerWarehouse].dbo.MeterChange m
						On c.ZoneId=m.ZoneId AND c.CustomerNumber=m.CustomerNumber
                    Where 
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        b.ZoneId in @zoneIds AND
						--c.DeletionStateId IN (0,1) AND
						c.ToDayJalali IS NULL AND
						(@FromDateJalali IS NULL OR
						@ToDateJalali IS NULL OR
						b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali) AND
						b.CounterStateCode=1
						)
                    SELECT * FROM CTE 
                    WHERE RN=1";
        }

        internal string GetGroupedQuery(string groupingField)
        {
            return @$";WITH CTE AS (
                    Select
                        b.ZoneTitle,
                        b.UsageTitle,
	                    b.SumItems,
                        b.Consumption,
						b.CommercialCount,
						b.DomesticCount,
						b.OtherCount,
						b.WaterDiameterId,
						b.CounterStateCode,
                        b.ConsumptionAverage,
	                    RN=ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay DESC)
                    From [CustomerWarehouse].dbo.Bills b
					Left Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
                    Where 
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						b.ZoneId IN @zoneIds AND
                        b.CounterStateCode NOT IN (4,7,8) AND
						c.DeletionStateId IN (0,1) AND
						c.ToDayJalali IS NULL AND
						(@FromDateJalali IS NULL OR
						@ToDateJalali IS NULL OR
						b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali) 
					)--cte
                    SELECT 
						c.{groupingField} as ItemTitle,
						SUM(c.SumItems) as SumItems,
						AVG(c.ConsumptionAverage) as Consumption,
						COUNT(1) AS CustomerCount,
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
					FROM CTE c
                    WHERE 
						c.RN=1 AND
						c.CounterStateCode=1 
					Group By c.{groupingField}";
        }

		//TODO: انشعابات جمع آوری شده جهت تطبیق گزارش ها موقتا نادیده گرفته شده
        internal string GetGroupedQueryNonDuplicate(string groupingField)
        {
            return @$";WITH CTE AS (
                    Select
                        b.ZoneTitle,
						b.ZoneId,
                        b.UsageTitle,
	                    b.SumItems,
                        b.Consumption,
						b.CommercialCount,
						b.DomesticCount,
						b.OtherCount,
						b.WaterDiameterId,
						b.CounterStateCode,
                        b.ConsumptionAverage,
	                    RN=ROW_NUMBER() OVER (PARTITION BY b.BillId ORDER BY b.RegisterDay DESC)
                    From [CustomerWarehouse].dbo.Bills b
					Left Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
                    Where 
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						b.ZoneId IN @zoneIds AND
                        b.CounterStateCode=1 AND
						--c.DeletionStateId IN (0,1) AND
						c.ToDayJalali IS NULL AND
						(@FromDateJalali IS NULL OR
						@ToDateJalali IS NULL OR
						b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali) 
					)--cte
                    SELECT 
				    	MAX(t46.C2) AS RegionTitle,
						c.{groupingField} as ItemTitle,
						SUM(c.SumItems) as SumItems,
						AVG(c.ConsumptionAverage) as Consumption,
						COUNT(1) AS CustomerCount,
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
					FROM CTE c
				    Join [Db70].dbo.T51 t51
				    	On t51.C0=c.ZoneId
				    Join [Db70].dbo.T46 t46
				    	On t51.C1=t46.C0
                    WHERE 
						c.RN=1 AND
						c.CounterStateCode=1 
					Group By c.{groupingField}";
        }
    }
}
