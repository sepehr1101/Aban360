using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class SewageWaterDistanceofRequestAndInstallationBase : AbstractBaseConnection
    {
        public SewageWaterDistanceofRequestAndInstallationBase(IConfiguration configuration)
            : base(configuration)
        { 
        }

        internal string GetDetailsQuery(bool isWater, bool isInstallation)
        {
            QueryParams queryParams = GetQueryParams(isWater, isInstallation);
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where			
                            c.{queryParams.DataField} BETWEEN @FromDateJalali AND @ToDateJalali AND
                            c.{queryParams.RegisterField} IS NOT NULL AND
                            TRIM(c.{queryParams.RegisterField}) != '' AND		                    
		                    c.ZoneId IN @zoneIds AND
		                    c.UsageId IN @usageIds AND
		                    (
			                    @fromReadingNumber IS NULL OR 
			                    @toReadingNumber IS NULL OR
			                    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
		                    ) AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @ToDateJalali
                    )
                    Select	
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
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.{queryParams.DataField} AS RequestDate,
						c.{queryParams.RegisterField} AS InstallationDate
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
	                    c.DeletionStateId NOT IN(1,2)";
        }

        internal string GetGroupedQuery(bool isWater, bool isInstallation, string groupingField)
        {
            QueryParams queryParams = GetQueryParams(isWater, isInstallation);

            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where			
                            c.{queryParams.DataField} BETWEEN @FromDateJalali AND @ToDateJalali AND
                            c.{queryParams.RegisterField} IS NOT NULL AND
                            TRIM(c.{queryParams.RegisterField}) != '' AND		                    
		                    c.ZoneId IN @zoneIds AND
		                    c.UsageId IN @usageIds AND
		                    (
			                    @fromReadingNumber IS NULL OR 
			                    @toReadingNumber IS NULL OR
			                    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
		                    ) AND
		                    c.CustomerNumber<>0 AND
		                    c.RegisterDayJalali <= @ToDateJalali
                    )
                    SELECT
                        MAX(t46.C2) AS RegionTitle,
                    	c.{groupingField} AS ItemTitle,
                    	c.{groupingField} ,
						Count(c.{groupingField}) as CustomerCount,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY,
                            Case When LEN(c.{queryParams.DataField})=10 Then [CustomerWarehouse].dbo.PersianToMiladi(c.{queryParams.DataField}) END,
                            Case When LEN(c.{queryParams.RegisterField})=10 Then [CustomerWarehouse].dbo.PersianToMiladi(c.{queryParams.RegisterField}) END))), 2) AS DistanceAverage
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
	                    c.DeletionStateId NOT IN(1,2)
                    GROUP BY
                        c.{groupingField}";                       
        }

        private QueryParams GetQueryParams(bool isWater, bool isInstallation)
        {
            string PhysicalWaterInstallDateJalali = nameof(PhysicalWaterInstallDateJalali),
                   PhysicalSewageInstallDateJalali = nameof(PhysicalSewageInstallDateJalali),
                   WaterRequestDate = nameof(WaterRequestDate),
                   SewageRequestDate = nameof(SewageRequestDate),
                   WaterRegisterDateJalali = nameof(WaterRegisterDateJalali),
                   SewageRegisterDateJalali = nameof(SewageRegisterDateJalali);

            string requestField = isWater ? WaterRequestDate : SewageRequestDate;
            string registerField = isWater ? WaterRegisterDateJalali : SewageRegisterDateJalali;
            string installField = isWater ? PhysicalWaterInstallDateJalali : PhysicalSewageInstallDateJalali;
            string dataField = isInstallation ? installField : requestField;


            return new QueryParams(dataField, registerField);
        }
        private record QueryParams
        {
            public string DataField { get; set; } = default!;
            public string RegisterField { get; set; } = default!;
            public QueryParams(string dataField, string registerField)
            {
                DataField = dataField;
                RegisterField = registerField;
            }
            public QueryParams()
            {

            }
        }
    }
}
