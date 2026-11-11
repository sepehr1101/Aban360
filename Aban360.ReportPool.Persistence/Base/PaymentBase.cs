using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class PaymentBase : AbstractBaseConnection
    {
        public PaymentBase(IConfiguration configuration)
            : base(configuration)
        {
        }

        internal string GetDetailQuery(bool isWater,bool hasZone)
        {
            QueryParam parameters= GetQueryParam(isWater, hasZone);

            return @$"Select
						p.ZoneTitle,
                     	p.CustomerNumber As CustomerNumber,
                    	p.PayDateJalali AS BankDateJalali,
                    	p.BankCode AS BankCode,
                    	p.RegisterDay AS EventBankDateJalali,
                    	p.BillId AS BillId,
                    	p.PaymentGateway AS PaymentMethodTitle,
                    	p.RegisterDay AS PaymentDate,
                    	CAST(p.Amount as bigint) AS Amount,
                        p.BankName AS BankName
                    From [CustomerWarehouse].dbo.{parameters.TableField} p
                    WHERE 
                    	(@FromDateJalali IS  NULL 
                    		OR @ToDateJalali IS NULL 
                    		OR p.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali) 
                    	AND(@FromAmount IS  NULL 
                    		OR @ToAmount IS NULL 
                    		OR p.Amount BETWEEN @FromAmount AND @ToAmount)
                        AND(@fromBankId IS NULL OR
						    @toBankId IS NULL OR
						    p.BankCode BETWEEN @fromBankId AND @toBankId)
                        {parameters.QueryCondition}
					Order By 
							 p.ZoneTitle,
                             p.BankName,
                             p.BankCode,
                             p.Amount Desc";

        }

        internal string GetGroupedQuery(bool isWater,bool hasZone,string groupingField)
        {
            QueryParam parameters = GetQueryParam(isWater, hasZone);

                return $@"Select 
						     '' AS RegionTitle,
                    	    SUM(p.Amount) AS Amount,
                    	    p.{groupingField} AS ItemTitle,
                    	    p.{groupingField} ,
						    COUNT(p.{groupingField}) AS CustomerCount,
						    /*SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0))*/0 AS TotalUnit,
						    /*SUM(ISNULL(c.CommercialCount, 0))*/0 AS CommercialUnit,
						    /*SUM(ISNULL(c.DomesticCount, 0))*/0 AS DomesticUnit,
						    /*SUM(ISNULL(c.OtherCount, 0))*/0 AS OtherUnit,
						    SUM(CASE WHEN p.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						    SUM(CASE WHEN p.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
						    SUM(CASE WHEN p.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
						    SUM(CASE WHEN p.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
						    SUM(CASE WHEN p.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
						    SUM(CASE WHEN p.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
						    SUM(CASE WHEN p.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
						    SUM(CASE WHEN p.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
						    SUM(CASE WHEN p.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
						    SUM(CASE WHEN p.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
						    SUM(CASE WHEN p.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                        From [CustomerWarehouse].dbo.{parameters.TableField} p	
                        WHERE                           
                    	    p.RegisterDay BETWEEN @FromDateJalali and @ToDateJalali
                            AND (@fromBankId IS NULL OR
						        @toBankId IS NULL OR
						        p.BankCode BETWEEN @fromBankId AND @toBankId)
                            {parameters.QueryCondition}
                        GROUP BY p.{groupingField}
					    Order By
						    --MAX(t46.C2),
						    p.{groupingField}";
        }

        private QueryParam GetQueryParam(bool isWater, bool hasZone)
        {
            string Payments = nameof(Payments),
                   PaymentsEn = nameof(PaymentsEn);

            string zoneQuery = hasZone ? "AND p.ZoneId IN @ZoneIds" : string.Empty;
            string tableField = isWater ? Payments : PaymentsEn;

            return new QueryParam(zoneQuery, tableField);
        }

        private record QueryParam
        {
            public string QueryCondition { get; set; }
            public string TableField { get; set; }
            public QueryParam(string queryCondition,string tableField)
            {
                QueryCondition= queryCondition;
                TableField= tableField;
            }
            public QueryParam()
            {

            }
        }
    }
}
