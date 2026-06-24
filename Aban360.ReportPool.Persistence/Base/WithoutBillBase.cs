using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    //این گزارش بنظر من دارای اسم نا مناسب است
    // اسم بهتر برای این گزارش بدون قرائت است و نه بدون صدور قبض
    // بابت اینکه هر دو حالت بدون صدور قبض و بدون قرائت را پوشش دهد میتوان از کد زیر استفاده کرد
    //b.TypeCode IN (1,7,8) -> b.TypeCode IN(1)
    internal abstract class WithoutBillBase : AbstractBaseConnection
    {
        public WithoutBillBase(IConfiguration configuration)
            : base(configuration)
        { 
        }
        internal string GetDetailQuery(WithoutBillInputDto input)
        {
            bool hasZone = input.ZoneIds.HasValue();
            bool hasUsage = input.UsageIds.HasValue();
            Parameters parameters = GetQueryParam(input,hasZone, hasUsage);

            return $@";With WithoutBill as (
                    Select 
                    	c.ZoneId,
                    	c.BillId,
                    	c.WaterRequestDate AS MeterRequestDateJalali,
                    	c.WaterRegisterDateJalali AS MeterInstallationDateJalali,
                    	c.MobileNo as MobileNumber,
                    	c.PhoneNo as PhoneNumber,
                    	c.ContractCapacity as ContractualCapacity,
                    	ISNULL(c.CommercialCount, 0) as CommercialUnit,
                    	ISNULL(c.DomesticCount, 0) as DomesticUnit,
                    	ISNULL(c.OtherCount, 0) as OtherUnit,
                    	(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) as TotalUnit,
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
                    	c.ZoneTitle as ZoneTitle,
                        c.DeletionStateId,
                        c.HasWater
                    From [CustomerWarehouse].dbo.Clients c
                    Where NOT EXISTS
                    (
                    	Select 1
                    	From [CustomerWarehouse].dbo.Bills  b
                    	Where 
                    		b.NextDay BETWEEN @FromDateJalali and @ToDateJalali AND
                    		b.TypeCode IN (1,7,8) AND
                    		c.CustomerNumber=b.CustomerNumber AND
                    		c.ZoneId=b.ZoneId
                    ) AND
                    c.ToDayJalali IS NULL AND
                    c.DeletionStateId IN (0) AND 
                    c.WaterRegisterDateJalali < '{parameters.ThresholdDate}'
                    {parameters.CReadingCondition}
                    {parameters.CZoneQuery}
                    {parameters.CUsageQuery}
                    ),
					LatestBill as
                    (
                        SELECT  
							b.ZoneId,
							b.CustomerNumber,
							b.CounterStateTitle,
							b.NextDay,
							b.RegisterDay,
							b.TypeCode,
							ROW_NUMBER() OVER(PARTITION BY ZoneId, CustomerNumber ORDER BY RegisterDay DESC) AS RN
						FROM [CustomerWarehouse].dbo.Bills b
						WHERE 
						    b.TypeCode IN (1,7,8) 
                            {parameters.BReadingCondition}
                            {parameters.BZoneQuery}
                            {parameters.BUsageQuery}
					)
                    Select 
                    	b.RegisterDay as LatestBillDateJalali,
                    	b.NextDay as LatestReadingDateJalali,
                    	b.CounterStateTitle AS CounterStateTitle,
                    	w.BillId AS BillId,
                    	w.ZoneId,
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
                    	w.ZoneTitle
                    From WithoutBill w
                    Left Join LatestBill b
                    	On w.ZoneId=b.ZoneId AND w.CustomerNumber=b.CustomerNumber AND RN=1
                    WHERE 
                        w.HasWater = 1";
        }
        internal string GetGroupedQuery(WithoutBillInputDto input, bool isZone)
        {
            bool hasZone = input.ZoneIds.HasValue();
            bool hasUsage = input.UsageIds.HasValue();
            Parameters parameters = GetQueryParam(input, hasZone, hasUsage, isZone);

            return $@";With WithoutBill as (
                    Select 
                    	c.ZoneId,
						c.ZoneTitle,
						c.UsageTitle,
						c.CustomerNumber,
						c.WaterDiameterId,
                    	c.CommercialCount,
                    	c.DomesticCount,
                    	c.OtherCount,
                        c.DeletionStateId,
                        c.HasWater
                    From [CustomerWarehouse].dbo.Clients c
                    Where NOT EXISTS
                    (
                    	Select 1
                    	From [CustomerWarehouse].dbo.Bills b
                    	Where 
                    		b.NextDay BETWEEN @FromDateJalali and @ToDateJalali AND
                    		b.TypeCode IN (1,7,8) AND
                    		c.CustomerNumber=b.CustomerNumber AND
                    		c.ZoneId=b.ZoneId
                    ) AND
                    c.ToDayJalali IS NULL AND
                    c.DeletionStateId IN (0) AND 
                    c.WaterRegisterDateJalali < '{parameters.ThresholdDate}'
                    {parameters.CReadingCondition}
                    {parameters.CZoneQuery}
                    {parameters.CUsageQuery}
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
                    Join [Db70].dbo.T51 t51
                    	On t51.C0=w.ZoneId
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
                    WHERE 
                        w.HasWater=1
					Group By w.{parameters.ZoneOrUsageGrouped}";
        }
        internal string GetGroupedBothQuery(WithoutBillInputDto input)
        {
            Parameters parameters = GetQueryParam(input, true, true, false);

            string ZoneTitle = nameof(ZoneTitle),
                  UsageTitle = nameof(UsageTitle);

            return $@";With WithoutBill as (
                    Select 
                    	c.ZoneId,
						c.ZoneTitle,
						c.UsageTitle,
						c.CustomerNumber,
						c.WaterDiameterId,
                    	c.CommercialCount,
                    	c.DomesticCount,
                    	c.OtherCount,
                        c.DeletionStateId,
                        c.HasWater
                    From [CustomerWarehouse].dbo.Clients c
                    Where 
                    NOT EXISTS(
                    	Select 1
                    	From [CustomerWarehouse].dbo.Bills  b
                    	Where 
                    		b.NextDay BETWEEN @FromDateJalali and @ToDateJalali AND
                    		b.TypeCode IN (1,7,8) AND
                    		c.CustomerNumber=b.CustomerNumber AND
                    		c.ZoneId=b.ZoneId
                    ) AND
                    c.ToDayJalali IS NULL AND
                    c.DeletionStateId IN (0) AND 
                    c.WaterRegisterDateJalali < '{parameters.ThresholdDate}'
                    {parameters.CReadingCondition}
                    {parameters.CZoneQuery}
                    {parameters.CUsageQuery}
                    )
                    Select 
						MAX(t46.C2) AS RegionTitle,
                    	w.{ZoneTitle},
                    	w.{UsageTitle},
						COUNT(1) AS CustomerCount,
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
                    Join [Db70].dbo.T51 t51
                    	On t51.C0=w.ZoneId
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
                    WHERE 
                        w.HasWater=1
					Group By w.{ZoneTitle}, w.{UsageTitle}
                    ORDER BY w.{ZoneTitle}, w.{UsageTitle}";
        }

        private Parameters GetQueryParam(WithoutBillInputDto input, bool hasZone, bool hasUsage, bool isZone=false)
        {
            string ZoneTitle = nameof(ZoneTitle),
                   UsageTitle = nameof(UsageTitle);
            DateTime toDateMiladi = input.ToDateJalali.ToGregorianDateTime().Value;
            return new Parameters()
            {
                BZoneQuery = hasZone ? "AND b.ZoneId IN @ZoneIds" : string.Empty,
                CZoneQuery = hasZone ? "AND c.ZoneId IN @ZoneIds" : string.Empty,
                BUsageQuery = hasUsage ? "AND b.UsageId IN @usageIds" : string.Empty,
                CUsageQuery = hasUsage ? "AND c.UsageId IN @usageIds" : string.Empty,
                ZoneOrUsageGrouped = isZone ? ZoneTitle : UsageTitle,
                ThresholdDate = toDateMiladi.AddDays(-45).ToShortPersianDateString(),
                CReadingCondition = string.IsNullOrWhiteSpace(input.FromReadingNumber) || string.IsNullOrWhiteSpace(input.ToReadingNumber) ? string.Empty:
                                    "AND c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber" ,
                BReadingCondition = string.IsNullOrWhiteSpace(input.FromReadingNumber) || string.IsNullOrWhiteSpace(input.ToReadingNumber) ? string.Empty:
                                    "AND b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber"
            };
        }

        private record Parameters
        {
            public string? BZoneQuery { get; set; }
            public string? CZoneQuery { get; set; }
            public string? BUsageQuery { get; set; }
            public string? CUsageQuery { get; set; }
            public string? ZoneOrUsageGrouped { get; set; }
            public string ThresholdDate { get; set; } = default!;
            public string? CReadingCondition { get; set; }
            public string? BReadingCondition { get; set; }
        }
    }
}
