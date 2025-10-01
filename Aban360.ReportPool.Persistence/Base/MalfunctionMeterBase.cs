using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class MalfunctionMeterBase : AbstractBaseConnection
    {
        public MalfunctionMeterBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery()
        {
            return @";WITH CTE AS (
                    Select
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
					Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
					Join [CustomerWarehouse].dbo.MeterChange m
						On c.ZoneId=m.ZoneId AND c.CustomerNumber=m.CustomerNumber
                    Where 
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        b.ZoneId in @zoneIds AND
                        b.CounterStateCode NOT IN (4,7,8) AND
						c.DeletionStateId IN (0,2) AND
						(@fromDate IS NULL OR
						@toDate IS NULL OR
						b.RegisterDay BETWEEN @fromDate AND @toDate)
						)
                    SELECT * FROM CTE 
                    WHERE RN=1 AND CounterStateCode=1";
        }

        internal string GetGroupedQuery(bool isZone)
        {
            string parameters = GetQueryParam(isZone);

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
					Join [CustomerWarehouse].dbo.Clients c 
						ON b.CustomerNumber=c.CustomerNumber AND b.ZoneId=c.ZoneId
                    Where 
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						b.ZoneId IN (131211,131301) AND
                        b.CounterStateCode NOT IN (4,7,8) AND
						c.DeletionStateId IN (0,2) AND
						(@fromDate IS NULL OR
						@toDate IS NULL OR
						b.RegisterDay BETWEEN @fromDate AND @toDate))
                    SELECT 
						{parameters} as ItemTitle,
						SUM(c.SumItems) as SumItems,
						AVG(c.ConsumptionAverage) as Consumption,
						COUNT(c.{parameters}) AS CustomerCount,
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
					Group By c.{parameters}";
        }

        private string GetQueryParam( bool isZone )
        {
            string ZoneTitle = nameof(ZoneTitle),
                   UsageTitle = nameof(UsageTitle);

            return isZone ? ZoneTitle : UsageTitle;
        }
    }
}
