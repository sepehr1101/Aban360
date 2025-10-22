using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class RuinedMeterIncomeBase : AbstractBaseConnection
    {
        public RuinedMeterIncomeBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery()
        {
            return @"Select 
	                    b.BillId,
                        b.ZoneTitle,
                        b.ZoneId,
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
						b.WaterDiameterTitle,
                        b.CommercialCount AS CommercialUnit,
                        b.DomesticCount AS DomesticUnit,
                        b.OtherCount AS OtherUnit,
                        IIF((b.DomesticCount+b.CommercialCount +b.OtherCount=0) ,1, (b.DomesticCount+b.CommercialCount +b.OtherCount)) AS TotalUnit,
						b.UsageTitle
                    From [CustomerWarehouse].dbo.Bills b
                    Where                     	
                        (@FromReadingNumber IS NULL or
                    	@ToReadingNumber IS NULL or 
                    	b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	b.ZoneId IN @zoneIds AND
                    	b.CounterStateCode=1";
        }

        internal string GetGroupedQuery(string groupingField)
        {
            return @$";WITH MalfunctionIncome as(
                    Select 
                    	 b.UsageTitle,
                         b.UsageId,
                         b.ZoneId,
                         b.ZoneTitle,
						 b.WaterDiameterId,
						 b.SumItems,
						 b.Payable,
						 b.CommercialCount,
						 b.DomesticCount,
						 b.OtherCount,
						 b.Consumption,
						 b.Duration	
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                        (@FromReadingNumber IS NULL or
                    	@ToReadingNumber IS NULL or 
                    	b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	b.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	b.ZoneId IN @zoneIds AND
                    	b.CounterStateCode=1)
                    Select	
						MAX(t46.C2) AS RegionTitle,		
						m.{groupingField} as ItemTitle,		
						m.{groupingField},		
						COUNT(m.{groupingField}) AS CustomerCount,
						SUM(m.Duration) AS SumDuration,
						AVG(m.Duration) AS AverageDuration,
						SUM(m.Consumption) AS SumConsumption,
						AVG(m.Consumption) AS AverageConsumption,
					    SUM(m.Payable) AS Payable,
					    SUM(m.SumItems) AS SumItems,
					    SUM(ISNULL(m.CommercialCount, 0) + ISNULL(m.DomesticCount, 0) + ISNULL(m.OtherCount, 0)) AS TotalUnit,
					    SUM(ISNULL(m.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(m.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(m.OtherCount, 0)) AS OtherUnit,
						SUM(CASE WHEN m.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						SUM(CASE WHEN m.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
						SUM(CASE WHEN m.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
						SUM(CASE WHEN m.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
						SUM(CASE WHEN m.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
						SUM(CASE WHEN m.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
						SUM(CASE WHEN m.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
						SUM(CASE WHEN m.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
						SUM(CASE WHEN m.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
						SUM(CASE WHEN m.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
						SUM(CASE WHEN m.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From MalfunctionIncome m
                    Join [Db70].dbo.T51 t51
						On t51.C0=m.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Group By m.{groupingField}";
        }
    }
}