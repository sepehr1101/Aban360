using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class NonPermanentBranchBase : AbstractBaseConnection
    {
        public NonPermanentBranchBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery()
        {
            return @"SELECT 
                        c.CustomerNumber,
                        c.ReadingNumber,
                        TRIM(c.FirstName) AS FirstName,
                        TRIM(c.SureName) As Surname,
                        c.UsageTitle,
                        c.WaterDiameterTitle MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                        c.RegisterDayJalali AS EventDateJalali,
                        c.WaterInstallDate AS WaterInstallationDate,
                        0 AS DebtAmount,
                        TRIM(c.Address) AS Address,
                        c.ZoneTitle,
                        c.DeletionStateId,
                        c.DeletionStateTitle AS UseStateTitle,
                        c.DomesticCount DomesticUnit,
            	        c.CommercialCount CommercialUnit,
            	        c.OtherCount OtherUnit,
                        (c.DomesticCount+c.CommercialCount +c.OtherCount) AS TotalUnit,
                    	c.ContractCapacity AS ContractualCapacity,
            	        TRIM(c.BillId) BillId
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
            			c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						 @toReadingNumber IS NULL OR
						 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                        c.ZoneId in @ZoneIds AND
						c.IsNonPermanent=1 AND
                        (@fromDate IS NULL OR
                        @toDate IS NULL OR
                        c.RegisterDayJalali BETWEEN @fromDate AND @toDate)";
        }

        internal string GetGroupedQuery(bool isTwoField,string firstGroupingField,string? secondGroupingField)
        {
            string multipleGroupedField = $@"c.{firstGroupingField},c.{secondGroupingField}";
            string singleGroupedField = $@"c.{firstGroupingField}";
            string groupingQury = isTwoField ?multipleGroupedField:singleGroupedField;
           
            return $@"SELECT 
                        {groupingQury},
						COUNT(c.{firstGroupingField}) AS CustomerCount,
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
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
            			c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						 @toReadingNumber IS NULL OR
						 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						 (c.RegisterDayJalali BETWEEN @fromDate AND @toDate) AND
						c.ZoneId in @zoneIds AND
                        c.UsageId in @usageIds AND
						c.IsNonPermanent=1
					Group By 
						{groupingQury}";
        }

        internal string GetSummaryQuer()
        {
            return @"SELECT 
                        c.ZoneTitle,
						c.UsageTitle,
						c.WaterDiameterTitle AS MeterDiameterTitle,
						Count(1) AS Count
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
            			c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						 @toReadingNumber IS NULL OR
						 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ZoneId in @zoneIds AND
						c.IsNonPermanent=1
					Group By 
						c.ZoneTitle ,
						c.UsageTitle,
						c.WaterDiameterTitle";
        }
    }
}
