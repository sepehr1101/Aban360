using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class SewageWaterDistanceofRequestAndInstallationBase : AbstractBaseConnection
    {
        public SewageWaterDistanceofRequestAndInstallationBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool isWater, bool isInstallation)
        {
            QueryParams queryParams = GetQueryParams(isWater, isInstallation);

            return @$"Select
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
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                        c.{queryParams.DataField} IS NOT NULL AND
                        TRIM(c.{queryParams.DataField}) != '' AND
                        c.{queryParams.RegisterField} IS NOT NULL AND
                        TRIM(c.{queryParams.RegisterField}) != '' AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	c.{queryParams.RegisterField} BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL";
        }

        internal string GetGroupedQuery(bool isWater, bool isInstallation, string groupingField)
        {
            QueryParams queryParams = GetQueryParams(isWater, isInstallation);

            return $@"Select	
						MAX(t46.C2) AS RegionTitle,
                    	c.{groupingField} AS ItemTitle,
                    	c.{groupingField} ,
						ROUND(AVG(CONVERT(float, DATEDIFF(DAY,
                        Case When LEN(c.{groupingField})=10 Then [CustomerWarehouse].dbo.PersianToMiladi(c.{groupingField}) END,
                        Case When LEN(c.{groupingField})=10 Then [CustomerWarehouse].dbo.PersianToMiladi(c.{groupingField}) END))), 2) AS DistanceAverage
                    From [CustomerWarehouse].dbo.Clients c	
					Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where	
					    c.{groupingField} IS NOT NULL AND
					    c.{groupingField} IS NOT NULL AND
					    TRIM(c.{groupingField}) != '' AND
					    TRIM(c.{groupingField}) != '' AND
                    	c.{groupingField} BETWEEN @fromDate AND @toDate AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                    	c.ZoneId IN @zoneIds AND
            			c.ToDayJalali IS NULL
                    Group BY
                    	c.{groupingField}";
        }

        private QueryParams GetQueryParams(bool isWater, bool isInstallation)
        {
            string WaterInstallDate = nameof(WaterInstallDate),
                   SewageInstallDate = nameof(SewageInstallDate),
                   WaterRequestDate = nameof(WaterRequestDate),
                   SewageRequestDate = nameof(SewageRequestDate),
                   WaterRegisterDateJalali = nameof(WaterRegisterDateJalali),
                   SewageRegisterDateJalali = nameof(SewageRegisterDateJalali);

            string requestField = isWater ? WaterRequestDate : SewageRequestDate;
            string registerField = isWater ? WaterRegisterDateJalali : SewageRegisterDateJalali;
            string installField = isWater ? WaterInstallDate : SewageInstallDate;
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
