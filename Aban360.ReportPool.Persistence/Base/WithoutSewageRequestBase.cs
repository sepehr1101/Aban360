using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class WithoutSewageRequestBase : AbstractBaseConnection
    {
        public WithoutSewageRequestBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery()
        {
            return @"Select
                        c.CustomerNumber, 
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                        (c.DomesticCount+c.CommercialCount +c.OtherCount) AS TotalUnit ,
                    	c.BillId,
                    	c.BranchType AS BranchTypeTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.WaterRequestDate AS WaterRequestDate,
						c.WaterInstallDate AS WaterInstallationDate,
						c.WaterRegisterDateJalali AS WaterRegistrationDate,
						TRIM(c.PhoneNo) AS PhoneNumber,
						TRIM(c.MobileNo) AS MobileNumber,
						c.DeletionStateTitle AS UseStateTitle,
						TRIM(c.MeterSerialBody) AS BodySerial,
						TRIM(c.NationalId) AS NationalCode,
						TRIM(c.PostalCode) AS PostalCode
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterRegisterDateJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
						(TRIM(c.SewageRequestDate)='' OR c.SewageRequestDate IS NULL) AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)";
        }
        
        internal string GetGroupedQuery(string groupingField)
        {
            return $@"Select	
						MAX(t46.C2) AS RegionTitle,
                    	c.{groupingField} AS ItemTitle,
                    	c.{groupingField} ,
                        COUNT(c.{groupingField}) AS CustomerCount,
					    SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
                        SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
						SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
				        SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
				        SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
				        SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
				        SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
				        SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
				        SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
				        SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
				        SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
				        SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
				        SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From [CustomerWarehouse].dbo.Clients c
					Join [Db70].dbo.T5 t5
						On t5.C0=c.WaterDiameterId
                    Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where	
                    	c.WaterRegisterDateJalali BETWEEN @FromDateJalali AND @ToDateJalali AND
						(TRIM(c.SewageRequestDate)='' OR c.SewageRequestDate IS NULL) AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)
                    Group BY
                    	c.{groupingField}";
        }
    }
}
