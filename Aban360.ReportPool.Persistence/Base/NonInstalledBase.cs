using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class NonInstalledBase : AbstractBaseConnection
    {
        public NonInstalledBase(IConfiguration configuration)
            : base(configuration)
        { 
        }

        internal string GetDetailsQuery(bool isWater)
        {
            QueryParam param = GetQueryParam(isWater);
            return $@"
                    ;WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.{param.RequestDate} BETWEEN @FromDateJalali AND @ToDateJalali AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @toDate AND
                            c.ZoneId IN @zoneIds  AND
						    c.ToDayJalali IS NULL AND
						    (
                                @fromReadingNumber IS NULL OR
						        @toReadingNumber IS NULL OR
						        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                            )
                    )
                    Select	
	                  c.CustomerNumber, 
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) AS FirstName,
                    	TRIM(c.SureName) AS Surname,
                    	TRIM(c.Address) AS Address,
                    	c.UsageTitle AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,                       
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                    	c.ZoneTitle,
                    	c.ZoneId,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                        (c.DomesticCount+c.CommercialCount +c.OtherCount) AS TotalUnit ,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.{param.RequestDate} AS RequestDate,
						c.{param.RegisterDate} AS RegisterDateJalali,
                        c.{param.InstallDate} AS InstallationDateJalali
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
	                    c.DeletionStateId NOT IN(1,2) AND
						(c.{param.InstallDate})<='1330/01/01' ";
        }

        internal string GetGroupedQuery(bool isWater, string groupingField)
        {
            QueryParam param = GetQueryParam(isWater);
            return $@"
                    ;WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.{param.RequestDate} BETWEEN @FromDateJalali AND @ToDateJalali AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @toDate AND
                            c.ZoneId IN @zoneIds  AND
						    c.ToDayJalali IS NULL AND
						    (   @fromReadingNumber IS NULL OR
						        @toReadingNumber IS NULL OR
						        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                            )
                    )
                    Select	
	                    MAX(t46.C2) AS RegionTitle,
                    	c.{groupingField} as ItemTitle,
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
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
	                    c.DeletionStateId NOT IN(1,2) AND
						(c.{param.InstallDate})<='1330/01/01'
					GROUP BY c.{groupingField}";
        }

        private QueryParam GetQueryParam(bool isWater)
        {
            string WaterRequestDate = nameof(WaterRequestDate),
                   SewageRequestDate = nameof(SewageRequestDate),
                   WaterRegisterDateJalali = nameof(WaterRegisterDateJalali),
                   SewageRegisterDateJalali = nameof(SewageRegisterDateJalali),
                   WaterInstallDate = nameof(WaterInstallDate),
                   SewageInstallDate = nameof(SewageInstallDate);

            string requestDate = isWater ? WaterRequestDate : SewageRequestDate;
            string registerDate = isWater ? WaterRegisterDateJalali : SewageRegisterDateJalali;
            string installDate = isWater ? WaterInstallDate : SewageInstallDate;

            return new QueryParam(registerDate, installDate, requestDate);
        }

        private record QueryParam
        {
            public string RegisterDate { get; set; }
            public string InstallDate { get; set; }
            public string RequestDate { get; set; }
            public QueryParam(string registerDate, string instaledDate, string requestDate)
            {
                RegisterDate = registerDate;
                RequestDate = requestDate;
                InstallDate = instaledDate;
            }
            public QueryParam()
            {

            }
        }
    }
}
