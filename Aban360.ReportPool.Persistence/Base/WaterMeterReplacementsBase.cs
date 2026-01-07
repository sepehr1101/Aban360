using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class WaterMeterReplacementsBase : AbstractBaseConnection
    {
        public WaterMeterReplacementsBase(IConfiguration configuration)
            : base(configuration)
        { 
        }

        internal string GetDetailQuery(bool isChangeDate)
        {
            string dateParam = GetQueryParams(isChangeDate);

            return @$"Select 
                    	mc.CustomerNumber,
                        c.BillId,
                    	c.ReadingNumber,
                    	TRIM(c.FirstName) +' '+TRIM(c.SureName) AS FullName,
                    	c.UsageTitle2 AS UsageTitle,
                    	c.WaterDiameterTitle AS MeterDiameterTitle,
                        c.MainSiphonTitle AS SiphonDiameterTitle,
                    	mc.ChangeDateJalali AS MeterChangeDate,
                    	mc.RegisterDateJalali AS WaterRegistrationDate,
                    	TRIM(c.MeterSerialBody) AS BodySerial,
                    	c.ZoneTitle AS ZoneTitle,
                        mc.ChangeCauseTitle,
                    	c.DomesticCount	AS DomesticUnit,
                    	c.CommercialCount AS CommercialUnit,
                    	c.OtherCount AS OtherUnit,
                        (c.DomesticCount+c.CommercialCount +c.OtherCount) AS TotalUnit ,
                    	c.ContractCapacity AS ContractualCapacity
                    From [CustomerWarehouse].dbo.MeterChange mc
                    Join [CustomerWarehouse].dbo.Clients c 
                        on mc.CustomerNumber=c.CustomerNumber AND mc.ZoneId=c.ZoneId
                    Where 
                    	mc.{dateParam} BETWEEN @FromDateJalali AND @ToDateJalali AND
                    	c.ZoneId IN @zoneIds AND
                    	c.UsageId IN @UsageIds AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						c.ToDayJalali IS NULL 
                    Order By
                    	mc.RegisterDateJalali Desc,
                    	c.RegisterDayJalali Desc";
        }

        internal string GetGroupedQuery(bool isChangeDate, string groupingFiled)
        {
            string dateParam = GetQueryParams(isChangeDate);

            return $@"Select 
						MAX(t46.C2) AS RegionTitle,
                        {groupingFiled} AS ItemTitle,
                        {groupingFiled},
                        COUNT({groupingFiled}) AS CustomerCount,
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
                    From [CustomerWarehouse].dbo.MeterChange mc
                    Join [CustomerWarehouse].dbo.Clients c 
						on mc.CustomerNumber=c.CustomerNumber AND mc.ZoneId=c.ZoneId
                    Join [Db70].dbo.T51 t51
						On t51.C0=c.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
                    Where                         
                    	mc.{dateParam} BETWEEN @FromDateJalali AND @ToDateJalali AND
                        c.ToDayJalali IS NULL AND
                    	c.ZoneId IN @zoneIds AND
                    	c.UsageId IN @UsageIds AND
						(
                            @fromReadingNumber IS NULL OR
                            @toReadingNumber IS NULL OR
						    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber
                        )						
                    Group By {groupingFiled}";
        }

        private string GetQueryParams(bool isChangeDate)
        {
            string ChangeDateJalali = nameof(ChangeDateJalali),
                   RegisterDateJalali = nameof(RegisterDateJalali);

            return isChangeDate ? ChangeDateJalali : RegisterDateJalali;
        }

        private record QueryParams
        {
            public string GroupedField { get; set; } = default!;
            public string AliasTable { get; set; } = default!;
            public QueryParams(string groupedField, string aliasTable)
            {
                GroupedField = groupedField;
                AliasTable = aliasTable;
            }
            public QueryParams()
            {

            }

        }
    }
}
