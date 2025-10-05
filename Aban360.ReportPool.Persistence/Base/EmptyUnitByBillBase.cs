using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class EmptyUnitByBillBase : AbstractBaseConnection
    {
        public EmptyUnitByBillBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool hasZone, bool hasUsage)
        {
            string queryCondition = GetQueryCondition(hasZone, hasUsage);

            return @$";WITH EmptyUnitByBill AS
					(
					    SELECT
							b.CustomerNumber,
							b.ReadingNumber,
							TRIM(c.FirstName) AS FirstName,
							TRIM(c.SureName) As Surname,
							b.UsageTitle,
							b.WaterDiameterTitle MeterDiameterTitle,
							c.RegisterDayJalali AS EventDateJalali,
							TRIM(c.Address) AS Address,
							c.DeletionStateId,
							c.DeletionStateTitle AS UseStateTitle,
							b.DomesticCount DomesticUnit,
							b.CommercialCount CommercialUnit,
							b.OtherCount OtherUnit,
                            (c.CommercialCount+c.DomesticCount+c.OtherCount) AS TotalUnit,
                            c.MainSiphonTitle AS SiphonDiameterTitle,
                            c.ContractCapacity AS ContractualCapacity,
							TRIM(c.BillId) BillId,
							b.EmptyCount As EmptyUnit,
							b.ZoneId,
							b.ZoneTitle,
							c.NationalId AS NationalCode,
							c.PostalCode , 
							c.PhoneNo AS PhoneNumber,
						    TRIM(c.MobileNo) AS MobileNumber,
							c.FatherName ,
							b.Consumption,
							b.ConsumptionAverage,
							b.SumItems,
					        ROW_NUMBER() OVER (
					            PARTITION BY b.BillId
					            ORDER BY b.Id DESC
					        ) AS RowNum
					    FROM [CustomerWarehouse].dbo.Bills b
						Join [CustomerWarehouse].dbo.Clients c On b.CustomerNumber=c.CustomerNumber and b.ZoneId=c.ZoneId
					    WHERE
							(b.EmptyCount BETWEEN @fromUnit AND @toUnit)
							AND
							(@fromReadingNumber IS NULL OR
							@toReadingNumber IS NULL OR
							c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                            c.ToDayJalali is null
							{queryCondition}
					)
					SELECT 
						    e.CustomerNumber,
							e.ReadingNumber,
							e.FirstName,
							e.Surname,
							e.UsageTitle,
							e.MeterDiameterTitle,
							e.EventDateJalali,
							e.Address,
							e.OtherUnit,
							e.DeletionStateId,
							e.UseStateTitle,
							e.DomesticUnit,
							e.CommercialUnit,
							e.BillId,
							e.EmptyUnit,
							e.ZoneId,
							e.ZoneTitle,							
							t46.C2 AS RegionTitle,
							t46.C0 AS RegionId,
							e.NationalCode,
							e.PostalCode , 
							e.PhoneNumber,
						    e.MobileNumber,
							e.FatherName ,
							e.Consumption,
							e.ConsumptionAverage,
							e.SumItems,
							e.ContractualCapacity ,
							e.SiphonDiameterTitle
					FROM EmptyUnitByBill e
					Join [Db70].dbo.T51 t51
						On t51.C0=e.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
					WHERE RowNum = 1;";
        }

        internal string GetGroupedQuery(bool hasZone, bool hasUsage, string groupingField)
        {
            string queryCondition= GetQueryCondition(hasZone, hasUsage);

            return @$";WITH EmptyUnitByBill AS
					(
					    SELECT
                            b.ZoneId,
							b.ZoneTitle,
							b.CommercialCount,
							b.DomesticCount,
							b.OtherCount,
							b.WaterDiameterId,
							b.EmptyCount AS EmptyUnit,
							RN=ROW_NUMBER() over (partition by b.BillId order by b.RegisterDay Desc)
					    FROM [CustomerWarehouse].dbo.Bills b
					    WHERE
							(b.EmptyCount BETWEEN @fromUnit AND @toUnit)
							AND
							(@fromReadingNumber IS NULL OR
							@toReadingNumber IS NULL OR
							b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) 
                            {queryCondition}
					)
					SELECT 
						MAX(t46.C2) AS RegionTitle,
					    e.{groupingField} AS ItemTitle,
					    e.{groupingField} ,
						COUNT(e.{groupingField}) AS CustomerCount,
					    SUM(ISNULL(e.CommercialCount, 0) + ISNULL(e.DomesticCount, 0) + ISNULL(e.OtherCount, 0)) AS TotalUnit,
				        SUM(ISNULL(e.CommercialCount, 0)) AS CommercialUnit,
				        SUM(ISNULL(e.DomesticCount, 0)) AS DomesticUnit,
				        SUM(ISNULL(e.OtherCount, 0)) AS OtherUnit,
						SUM(ISNULL(e.EmptyUnit,0)) AS EmptyUnit,
						SUM(CASE WHEN e.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						SUM(CASE WHEN e.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
						SUM(CASE WHEN e.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
						SUM(CASE WHEN e.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
						SUM(CASE WHEN e.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
						SUM(CASE WHEN e.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
						SUM(CASE WHEN e.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
						SUM(CASE WHEN e.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
						SUM(CASE WHEN e.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
						SUM(CASE WHEN e.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
						SUM(CASE WHEN e.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
			         From EmptyUnitByBill e
					Join [Db70].dbo.T51 t51
						On t51.C0=e.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
				  	 Where e.RN=1
					 Group By e.{groupingField}";
        }

        private string GetQueryCondition(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? " AND b.ZoneId in @zoneIds" : string.Empty;
            string usageQuery = hasUsage ? " AND b.UsageId in @usageIds" : string.Empty;

            return zoneQuery + usageQuery;
        }
    }
}
