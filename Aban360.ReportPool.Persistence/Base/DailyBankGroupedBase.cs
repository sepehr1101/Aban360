using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class DailyBankGroupedBase : AbstractBaseConnection
    {
        public DailyBankGroupedBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool hasZone, string tableField)
        {
            string zoneQuery = hasZone ? "AND p.ZoneId IN @ZoneIds" : string.Empty;

            return @$"Select 
                    	p.RegisterDay AS RegisterDate, 
                    	p.PayDateJalali AS BankDate,
                        p.ZoneTitle AS ZoneTitle,
                        p.BankName AS BankName,
                        p.BankCode AS BankCode,
                    	COUNT(p.RegisterDay) AS ItemCount,
                    	SUM(p.Amount) AS ItemAmount,
                    	COUNT(p.RegisterDay) AS TotalCount,
                    	SUM(p.Amount) AS TotalAmount
                    From [CustomerWarehouse].dbo.{tableField} p
                    WHERE 
                    	(
                            (@FromDate IS NOT NULL AND @ToDate IS NOT NULL AND p.RegisterDay BETWEEN @FromDate AND @ToDate)
                            OR (@FromDate IS NULL AND @ToDate IS NULL)
                        )AND
                        (
                            (@FromAmount IS NOT NULL AND @ToAmount IS NOT NULL AND p.Amount BETWEEN @FromAmount AND @ToAmount)
                            OR (@FromAmount IS NULL AND @ToAmount IS NULL)
                        )AND
						(@fromBankId IS NULL OR
						@toBankId IS NULL OR
						p.BankCode BETWEEN @fromBankId AND @toBankId)
                    {zoneQuery}
                    GROUP BY p.RegisterDay,
							 p.PayDateJalali,
                             p.BankName,
                             p.BankCode,
                             p.ZoneTitle";
        }

        internal string GetGroupedQuery()
        {
            return $@"";
        }
    }
}
