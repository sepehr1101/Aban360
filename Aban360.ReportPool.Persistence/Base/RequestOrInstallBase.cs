using Aban360.ReportPool.Domain.Constants;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class RequestOrInstallBase : AbstractBaseConnection
    {
        protected RequestOrInstallBase(IConfiguration configuration)
            : base(configuration)
        {
        }

        internal string GetDetailsQuery(bool isWater, InstallOrRequestOrInstallDepartmentEnum inputEnum)
        {
            //QueryParams queryParams = GetQueryParams(isWater, inputEnum);
            QueryParams queryParams = GetQueryParams(isWater, inputEnum);
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.{queryParams.DataField} BETWEEN @FromDateJalali AND @ToDateJalali AND
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
                        c.BlockCode,
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
                        IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount)) AS TotalUnit,
                    	c.BillId,
                    	c.BranchType AS UseStateTitle,
                    	c.ContractCapacity AS ContractualCapacity,
                    	c.{queryParams.RequestField} AS RequestDate,
                    	c.{queryParams.InstallField} AS InstallationDate,
                        c.{queryParams.RegisterField} AS RegisterDate
                    FROM CTE c
                    JOIN [Db70].dbo.T51 t51
	                    On t51.C0=c.ZoneId
                    JOIN [Db70].dbo.T46 t46
	                    On t51.C1=t46.C0
                    WHERE	  
                        c.RN=1 AND
	                    c.DeletionStateId NOT IN(1,2)";
        }
        internal string GetGroupedQuery(bool isWater, InstallOrRequestOrInstallDepartmentEnum inputEnum, string groupingField)// bool isRequest
        {
            //QueryParams queryParams = GetQueryParams(isWater, isRequest);
            QueryParams queryParams = GetQueryParams(isWater, inputEnum);
            return $@";WITH CTE AS
                    (
	                    SELECT 
		                    RN= ROW_NUMBER() OVER (PARTITION by ZoneId , CustomerNumber ORDER BY RegisterDayJalali DESC, LocalId DESC),
		                    *
                        From [CustomerWarehouse].dbo.Clients c
	                    Where				
		                    c.{queryParams.DataField} BETWEEN @FromDateJalali AND @ToDateJalali AND
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
	                    MAX(t46.C2) AS RegionTitle,
                        c.{groupingField},
                        c.{groupingField} as ItemTitle,
                        COUNT(1) AS CustomerCount,
	                    SUM(IIF((c.DomesticCount+c.CommercialCount +c.OtherCount=0) ,1, (c.DomesticCount+c.CommercialCount +c.OtherCount))) AS TotalUnit,
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
	                    c.DeletionStateId NOT IN(1,2)
                    GROUP BY
                        c.{groupingField}";
        }
        private QueryParams GetQueryParams(bool isWater, InstallOrRequestOrInstallDepartmentEnum inputEnum)// bool isRequest,
        {
            string WaterRequestDate = nameof(WaterRequestDate),
                   SewageRequestDate = nameof(SewageRequestDate),
                   WaterRegisterDateJalali = nameof(WaterRegisterDateJalali),
                   SewageRegisterDateJalali = nameof(SewageRegisterDateJalali),
                   WaterInstallDate = nameof(WaterInstallDate),
                   SewageInstallDate = nameof(SewageInstallDate);

            string requestField = isWater ? WaterRequestDate : SewageRequestDate;
            string registerField = isWater ? WaterRegisterDateJalali : SewageRegisterDateJalali;

            //string dataField = isRequest ? requestField : registerField;

            string dataField="";
            string installField = isWater ? WaterInstallDate : SewageInstallDate;
            if (inputEnum == InstallOrRequestOrInstallDepartmentEnum.Install)
                dataField = registerField;
            if (inputEnum == InstallOrRequestOrInstallDepartmentEnum.Request)
                dataField = requestField;
            if (inputEnum == InstallOrRequestOrInstallDepartmentEnum.InstallDepartment)
                dataField = installField;

                return new QueryParams(dataField, requestField, registerField,installField);//dataField
        }
        private record QueryParams
        {
            public string DataField { get; set; } = default!;
            public string RequestField { get; set; } = default!;
            public string RegisterField { get; set; } = default!;
            public string InstallField { get; set; } = default!;
            public QueryParams(string dataField, string requestField, string registerField,string installField)
            {
                DataField = dataField;
                RequestField = requestField;
                RegisterField = registerField;
                InstallField = installField;
            }
            public QueryParams()
            {

            }
        }
    }
}
