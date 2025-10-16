using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class UseStateBase : AbstractBaseConnection
    {
        public UseStateBase(IConfiguration configuration)
            : base(configuration)
        { 
        }

        internal string GetDetailQuery()
        {
            return @";WITH CTE AS 
                     (SELECT 
                        RN=ROW_NUMBER() OVER (PARTITION BY ZoneId, CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
                    	c.CustomerNumber,
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) As Surname,
                    	c.UsageTitle,
                    	c.WaterDiameterTitle MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                    	c.RegisterDayJalali AS EventDateJalali,
                    	0 AS DebtAmount,
                    	TRIM(c.Address) AS Address,
                    	c.ZoneTitle,
                    	c.DeletionStateId,
                        c.DeletionStateTitle AS DeletionStateTitle,
                        c.DomesticCount DomesticUnit,
	                    c.CommercialCount CommercialUnit,
                        (c.CommercialCount+c.DomesticCount+c.DomesticCount) as TotalUnit,
	                    c.OtherCount OtherUnit,
                    	c.ContractCapacity AS ContractualCapacity,
	                    TRIM(c.BillId) BillId,
						TRIM(c.MeterSerialBody) AS BodySerial,
						c.WaterRegisterDateJalali AS MeterInstallationDateJalali,
						c.WaterRequestDate AS MeterRequestDateJalali,
                        TRIM(c.PhoneNo) AS PhoneNumber,
						TRIM(c.MobileNo) AS MobileNumber,
						TRIM(c.NationalId) AS NationalCode,
						TRIM(c.PostalCode) AS PostalCode	                   
                    FROM [CustomerWarehouse].dbo.Clients c
                    WHERE 
                       c.RegisterDayJalali Between @FromDateJalali and @ToDateJalali AND
                       (   @fromReadingNumber IS NULL OR
						   @toReadingNumber IS NULL OR
						   c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                       ) and  
                        c.ZoneId in @zoneIds
                    ) 
                    SELECT * FROM CTE
                    WHERE 
                        RN=1 AND
                        DeletionStateId=@useStateId";
        }

        internal string GetGroupedQuery(string groupingField)
        {
            return $@";WITH CTE AS 
                     (SELECT 
                        RN=ROW_NUMBER() OVER (PARTITION BY ZoneId, CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
						c.UsageTitle,
                        c.ZoneTitle,
                        c.ZoneId,
						c.WaterDiameterId,
						c.CommercialCount,
						c.DomesticCount,
						c.OtherCount,
						c.DeletionStateId	                   
                    FROM [CustomerWarehouse].dbo.Clients c 
                    WHERE 
                       c.RegisterDayJalali Between @FromDateJalali and @ToDateJalali AND
                       (   @fromReadingNumber IS NULL OR
						   @toReadingNumber IS NULL OR
						   c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                       ) and  
                       c.ZoneId in @zoneIds
					)
                    SELECT 
						MAX(t46.C2) AS RegionTitle,
						c.{groupingField} AS ItemTitle,
						c.{groupingField},
                    	COUNT(c.{groupingField}) AS CustomerCount,
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
						c.DeletionStateId=@useStateId 
					Group By 
						c.{groupingField}";
        }

        internal string GetUseStateTitle()
        {
            return @"select Title
                     from [Aban360].ClaimPool.UseState 
                     where Id=@useStateId";
        }
    }
}
