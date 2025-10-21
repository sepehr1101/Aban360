using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class NonPermanentBranchBase : AbstractBaseConnection
    {
        public NonPermanentBranchBase(IConfiguration configuration)
            : base(configuration)
        {
        }

        internal string GetDetailsQuery()
        {
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
		                    c.ZoneId IN @zoneIds AND
		                    c.UsageId IN @usageIds AND
		                    (
			                    @fromReadingNumber IS NULL OR 
			                    @toReadingNumber IS NULL OR
			                    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
		                    ) AND
		                    c.CustomerNumber<>0
                    )
                    Select	
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
                        IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
                    	c.ContractCapacity AS ContractualCapacity,
            	        TRIM(c.BillId) BillId
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	                        
                        c.RN=1 AND
                        c.IsNonPermanent=1 AND
	                    c.DeletionStateId NOT IN(1,2)";
        }

        internal string GetGroupedQuery(string groupingField)
        {            
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
		                    c.ZoneId IN @zoneIds AND
		                    c.UsageId IN @usageIds AND
		                    (
			                    @fromReadingNumber IS NULL OR 
			                    @toReadingNumber IS NULL OR
			                    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
		                    ) AND
		                    c.CustomerNumber<>0
                    )
                    Select	
	                    MAX(t46.C2) AS RegionTitle,
                        {groupingField},
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
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
                        c.IsNonPermanent=1 AND
	                    c.DeletionStateId NOT IN(1,2)
                    GROUP BY
                        {groupingField}";
        }

        internal string GetSummaryQuery()
        {
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.RegisterDayJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
		                    c.ZoneId IN @zoneIds AND
		                    c.UsageId IN @usageIds AND
		                    (
			                    @fromReadingNumber IS NULL OR 
			                    @toReadingNumber IS NULL OR
			                    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
		                    ) AND
		                    c.CustomerNumber<>0
                    )
                    Select	
	                    c.ZoneTitle,
						c.UsageTitle,
						c.WaterDiameterTitle AS MeterDiameterTitle,
						Count(1) AS Count,
					    SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
					    SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
                        c.IsNonPermanent=1 AND
	                    c.DeletionStateId NOT IN(1,2)
                    GROUP BY
						c.ZoneTitle ,
						c.UsageTitle,
						c.WaterDiameterTitle";
        }
    }
}
