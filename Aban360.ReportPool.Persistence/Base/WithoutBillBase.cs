using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class WithoutBillBase : AbstractBaseConnection
    {
        public WithoutBillBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool hasZone, bool hasUsage)
        {
            Parameters parameters = GetQueryParam(hasZone, hasUsage);

            return $@";With WithoutBill as (
                    Select 
                    	c.ZoneId,
                    	c.BillId,
                    	c.WaterRequestDate AS MeterRequestDateJalali,
                    	c.WaterRegisterDateJalali AS MeterInstallationDateJalali,
                    	c.MobileNo as MobileNumber,
                    	c.PhoneNo as PhoneNumber,
                    	c.ContractCapacity as ContractualCapacity,
                    	c.CommercialCount as CommercialUnit,
                    	c.DomesticCount as DomesticUnit,
                    	c.OtherCount as OtherUnit,
                    	(c.ContractCapacity + c.DomesticCount + c.OtherCount) as TotalUnit,
                    	c.MainSiphonTitle as  SiphonDiameterTitle,
                    	c.UsageTitle as UsageTitle,
                    	TRIM(c.NationalId) as NationalCode,
                    	c.EmptyCount as EmptyUnit,
                    	c.CustomerNumber as CustomerNumber,
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) +' '+TRIM(c.SureName) as FullName,
                    	c.WaterDiameterTitle as MeterDiameterTitle,
                    	c.UsageTitle2 as UsageSellTitle,
                    	TRIM(c.Address) as Address,
                    	c.ZoneTitle as ZoneTitle
                    From [CustomerWarehouse].dbo.Clients c
                    Where NOT EXISTS(
                    		Select 1
                    		From [CustomerWarehouse].dbo.Bills  b
                    		Where 
                    			c.ZoneId=b.ZoneId AND
                    			c.CustomerNumber=b.CustomerNumber AND
                    			(@FromDateJalali IS NULL or
                    			@ToDateJalali IS NULL or 
                    			b.RegisterDay BETWEEN @FromDateJalali and @ToDateJalali)AND 
                    			 (@FromReadingNumber IS NULL or
                    			  @ToReadingNumber IS NULL or 
                    			  c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                            	c.DeletionStateId IN (0,2)  AND
                    			b.TypeCode = 1 AND
                    			c.ToDayJalali IS NULL 
                             {parameters.CZoneQuery}
                             {parameters.CUsageQuery}
                                )AND
                    		(@FromReadingNumber IS NULL or
                    			  @ToReadingNumber IS NULL or 
                    			  c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    			c.DeletionStateId IN (0,2)  AND
                    			c.ToDayJalali IS NULL 
                             {parameters.CZoneQuery}
                             {parameters.CUsageQuery}
                    ),
					LatestBill as(
						SELECT  
								b.ZoneId,
								b.CustomerNumber,
								b.CounterStateTitle,
								b.RegisterDay,
								b.NextDay,
								b.TypeCode,
								ROW_NUMBER() OVER(PARTITION BY ZoneId, CustomerNumber ORDER BY RegisterDay DESC) AS RN
						FROM [CustomerWarehouse].dbo.Bills b
						WHERE 
						  b.TypeCode = 1 AND
                    	(@FromReadingNumber IS NULL or
                    	 @ToReadingNumber IS NULL or 
                    	 b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) 
                             {parameters.BZoneQuery}
                             {parameters.BUsageQuery}
					)
                    Select 
                    	w.BillId AS BillId,
                    	w.ZoneId,
                    	b.CounterStateTitle AS CounterStateTitle,
                    	w.MeterRequestDateJalali,
                    	w.MeterInstallationDateJalali,
                    	w.MobileNumber,
                    	w.PhoneNumber,
                    	w.ContractualCapacity,
                    	w.CommercialUnit,
                    	w.DomesticUnit,
                    	w.OtherUnit,
                    	w.TotalUnit,
                    	w.SiphonDiameterTitle,
                    	w.UsageTitle,
                    	w.NationalCode,
                    	w.EmptyUnit,
                    	w.CustomerNumber,
                    	w.ReadingNumber,
                    	w.FullName,
                    	w.MeterDiameterTitle,
                    	w.UsageSellTitle,
                    	w.Address,
                    	w.ZoneTitle,
                    	b.RegisterDay as LatestBillDateJalali,
                    	b.NextDay as LatestReadingDateJalali
                    From WithoutBill w
                   Left Join LatestBill b
                    	On w.ZoneId=b.ZoneId AND w.CustomerNumber=b.CustomerNumber AND RN=1";
        }

        internal string GetGroupedQuery(bool hasZone, bool hasUsage, bool isZone)
        {
            Parameters parameters = GetQueryParam(hasZone, hasUsage,isZone);

            return $@";With WithoutBill as (
                    Select 
                    	c.ZoneId,
						c.ZoneTitle,
						c.UsageTitle,
						c.CustomerNumber,
						c.WaterDiameterId,
                    	c.CommercialCount,
                    	c.DomesticCount,
                    	c.OtherCount
                    From [CustomerWarehouse].dbo.Clients c
                    Where NOT EXISTS(
                    		Select 1
                    		From [CustomerWarehouse].dbo.Bills  b
                    		Where 
                    			c.ZoneId=b.ZoneId AND
                    			c.CustomerNumber=b.CustomerNumber AND
                    			(@FromDateJalali IS NULL or
                    			@ToDateJalali IS NULL or 
                    			b.RegisterDay BETWEEN @FromDateJalali and @ToDateJalali)AND 
                    			 (@FromReadingNumber IS NULL or
                    			  @ToReadingNumber IS NULL or 
                    			  c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                            	c.DeletionStateId IN (0,2)  AND
                    			b.TypeCode = 1 AND
                    			c.ToDayJalali IS NULL 
                             {parameters.CZoneQuery}
                             {parameters.CUsageQuery}
                                )AND
                    		(@FromReadingNumber IS NULL or
                    			  @ToReadingNumber IS NULL or 
                    			  c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    			c.DeletionStateId IN (0,2)  AND
                    			c.ToDayJalali IS NULL 
                             {parameters.CZoneQuery}
                             {parameters.CUsageQuery}
                    ),
					LatestBill as(
						SELECT  
								b.ZoneId,
								b.CustomerNumber,
								ROW_NUMBER() OVER(PARTITION BY ZoneId, CustomerNumber ORDER BY RegisterDay DESC) AS RN
						FROM [CustomerWarehouse].dbo.Bills b
						WHERE 
						  b.TypeCode = 1 AND
                    	(@FromReadingNumber IS NULL or
                    	 @ToReadingNumber IS NULL or 
                    	 b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) 
                         {parameters.BZoneQuery}
                         {parameters.BUsageQuery}
					)
                    Select 
						MAX(t46.C2) AS RegionTitle,
                    	w.{parameters.ZoneOrUsageGrouped} as ItemTitle,
                    	w.{parameters.ZoneOrUsageGrouped},
						COUNT(w.{parameters.ZoneOrUsageGrouped}) AS CustomerCount,
						SUM(ISNULL(w.CommercialCount, 0) + ISNULL(w.DomesticCount, 0) + ISNULL(w.OtherCount, 0)) AS TotalUnit,
						SUM(ISNULL(w.CommercialCount, 0)) AS CommercialUnit,
						SUM(ISNULL(w.DomesticCount, 0)) AS DomesticUnit,
						SUM(ISNULL(w.OtherCount, 0)) AS OtherUnit,
						SUM(CASE WHEN w.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						SUM(CASE WHEN w.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
						SUM(CASE WHEN w.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
						SUM(CASE WHEN w.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
						SUM(CASE WHEN w.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
						SUM(CASE WHEN w.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
						SUM(CASE WHEN w.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
						SUM(CASE WHEN w.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
						SUM(CASE WHEN w.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
						SUM(CASE WHEN w.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
						SUM(CASE WHEN w.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From WithoutBill w
                   Left Join LatestBill b
                    	On w.ZoneId=b.ZoneId AND w.CustomerNumber=b.CustomerNumber AND RN=1
                    Join [Db70].dbo.T51 t51
                    	On t51.C0=w.ZoneId
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
					Group By w.{parameters.ZoneOrUsageGrouped}";
        }

        private Parameters GetQueryParam(bool hasZone, bool hasUsage, bool isZone=false)
        {
            string ZoneTitle = nameof(ZoneTitle),
                   UsageTitle = nameof(UsageTitle);

            return new Parameters()
            {
                BZoneQuery = hasZone ? "AND b.ZoneId IN @ZoneIds" : string.Empty,
                CZoneQuery = hasZone ? "AND c.ZoneId IN @ZoneIds" : string.Empty,
                BUsageQuery= hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty,
                CUsageQuery= hasUsage ? "AND c.UsageId IN @usageIds" : string.Empty,
                ZoneOrUsageGrouped= isZone ? ZoneTitle : UsageTitle

            };
        }

        private record Parameters
        {
            public string? BZoneQuery { get; set; }
            public string? CZoneQuery { get; set; }
            public string? BUsageQuery { get; set; }
            public string? CUsageQuery { get; set; }
            public string? ZoneOrUsageGrouped { get; set; }
        }
    }
}
