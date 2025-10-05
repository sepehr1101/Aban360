using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class ReadingIssueDistanceBillBase:AbstractBaseConnection
    {
        public ReadingIssueDistanceBillBase(IConfiguration configuration)
            :base(configuration)
        {}

        internal string GetDetailQuery()
        {
            return @"Select
                	t46.C2 AS RegionTitle,
                	t46.C0 AS RegionId,
                	b.ZoneTitle,
                	b.ZoneId,
                	b.CustomerNumber,
                	b.ReadingNumber,
                	TRIM(b.BillId) as BillId ,
                	b.PreviousDay as PreviousDateJalali,
                	b.NextDay as CurrentDateJalali,
                	b.PreviousNumber,
                	b.NextNumber,
                	b.RegisterDay as RegisterDateJalali,
                	b.CounterStateTitle,
                	b.UsageTitle,
                	b.BranchType,
                	b.WaterDiameterId,
                	b.WaterDiameterTitle,
                	b.ContractCapacity as ContractualCapacity,
                	b.CommercialCount as CommercialUnit,
                	b.DomesticCount as DomesticUnit,
                	b.OtherCount as OtherUnit,
                	(b.CommercialCount+b.DomesticCount +b.OtherCount) as TotalUnit,
                	b.Consumption,
                	b.ConsumptionAverage,
                	b.ReadingStateTitle,
                	TRIM(c.FirstName) as FirstName,
                	TRIM(c.SureName) as Surname,
                	(TRIM(c.FirstName) + ' ' + TRIM(c.SureName)) as FullName,
                	TRIM(c.NationalId) as NationalCode,
                	TRIM(c.PostalCode) as PostalCode,
                	TRIM(c.PhoneNo) as PhoneNumber,
                	TRIM(c.MobileNo) as MobileNumber,
                	TRIM(c.Address) as Address
                From [CustomerWarehouse].dbo.Bills b
                Left Join [CustomerWarehouse].dbo.Clients c
                	On c.CustomerNumber=b.CustomerNumber AND c.ZoneId=b.ZoneId
                Join [Db70].dbo.T51 t51
                	On t51.C0=b.ZoneId
                Join [Db70].dbo.T46 t46
                	On t51.C1=t46.C0
                Where 
                    c.ToDayJalali IS NULL AND
                	b.TypeCode=1 AND
                	(@fromDate IS NULL OR
                	@toDate IS NULL OR
                	b.RegisterDay BETWEEN @fromDate AND @toDate) AND
                	(@fromReadingNumber IS NULL OR
                	@toReadingNumber IS NULL OR
                	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                	b.ZoneId IN @zoneIds";
        }
        internal string GetGroupedQuery(string groupingField)
        {
            return $@"Select
                        MAX(t46.C2) AS RegionTitle,
                      	b.{groupingField} as ItemTitle,
                      	b.{groupingField} ,
                      	COUNT(b.{groupingField}) as CustomerCount,
                      	SUM(ISNULL(b.CommercialCount, 0) + ISNULL(b.DomesticCount, 0) + ISNULL(b.OtherCount, 0)) AS TotalUnit,
                      	SUM(ISNULL(b.CommercialCount, 0)) AS CommercialUnit,
                      	SUM(ISNULL(b.DomesticCount, 0)) AS DomesticUnit,
                      	SUM(ISNULL(b.OtherCount, 0)) AS OtherUnit,
                      	AVG(CASE WHEN b.WaterDiameterId = 0 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS UnSpecified,
                      	AVG(CASE WHEN b.WaterDiameterId = 1 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field0_5,
                      	AVG(CASE WHEN b.WaterDiameterId = 2 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field0_75,
                      	AVG(CASE WHEN b.WaterDiameterId = 3 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field1,
                      	AVG(CASE WHEN b.WaterDiameterId = 4 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field1_2,
                      	AVG(CASE WHEN b.WaterDiameterId = 5 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field1_5,
                      	AVG(CASE WHEN b.WaterDiameterId = 6 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field2,
                      	AVG(CASE WHEN b.WaterDiameterId = 7 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field3,
                      	AVG(CASE WHEN b.WaterDiameterId = 8 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field4,
                      	AVG(CASE WHEN b.WaterDiameterId = 9 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field5,
                      	AVG(CASE WHEN b.WaterDiameterId In (10,11,12,13,15) THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS MoreThan6
                      From [CustomerWarehouse].dbo.Bills b
                      Join [Db70].dbo.T51 t51
                        	On t51.C0=b.ZoneId
                      Join [Db70].dbo.T46 t46
                      	    On t51.C1=t46.C0
                      Where 
                      	b.TypeCode=1 AND
                      	(@fromDate IS NULL OR
                      	@toDate IS NULL OR
                      	b.RegisterDay BETWEEN @fromDate AND @toDate) AND
                      	(@fromReadingNumber IS NULL OR
                      	@toReadingNumber IS NULL OR
                      	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                      	b.ZoneId IN @zoneIds
                     Group by b.{groupingField}";
        }
    }
}
