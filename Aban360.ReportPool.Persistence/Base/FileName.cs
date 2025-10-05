using Aban360.ReportPool.Domain.Base;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    //????????
    internal abstract class PaymentReceivableBase : AbstractBaseConnection
    {
        public PaymentReceivableBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool isWater, bool hasZone, bool hasUsage = false)
        {
            return isWater ? GetWaterQuery(hasZone) : GetServiceLinkQuery(hasZone, hasUsage);
        }

        internal string GetGroupedQuery()
        {
            return $@"";
        }

        private string GetWaterQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND p.ZoneId IN @ZoneIds" : string.Empty;
            return @$"Select 
                    	b.BillId,
						p.PayId,
						b.ZoneTitle,
                    	b.UsageTitle ,
						p.Amount,
						b.RegisterDay as BillIssueDateJalali,
						b.Deadline,
						p.PayDateJalali,
						IIF(p.PayDateJalali<=b.DeadLine,N'{ReportLiterals.Due}' , N'{ReportLiterals.Overdue}') AS AmountState
                    From [CustomerWarehouse].dbo.Bills b
                    LEFT JOIN [CustomerWarehouse].dbo.Payments p ON p.BillTableId=b.Id
                    WHERE
                        (@FromDate IS NULL
                     	    OR @ToDate IS NULL 
                     	    OR p.RegisterDay BETWEEN @FromDate AND @ToDate)
                        AND (@fromBankId IS NULL OR
						    @toBankId IS NULL OR
						    p.BankCode BETWEEN @fromBankId AND @toBankId)
                         {zoneQuery}";
        }
        
        private string GetServiceLinkQuery(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND ZoneId IN @ZoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND UsageId IN @UsageIds" : string.Empty;

            return @$"WITH OrderedData AS (
                    SELECT 
                        FullName, 
                		CustomerNumber,
                		ZoneId,
                		ZoneTitle,
                		UsageId,
                		UsageTitle,
                        EventDate,
                        IsPayed,
                        Amount,
                        SUM(Amount) OVER (
                            PARTITION BY ZoneId,CustomerNumber
                            ORDER BY EventDate
                            ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
                        ) AS SumAmount
                	from [CustomerWarehouse].dbo.PaymentDue 
                    Where 
                        (@fromDate IS NULL OR
                        @toDate IS NULL OR
                        EventDate BETWEEN @fromDate and @toDate) 
                        {zoneQuery}
                        {usageQuery}
                )
                SELECT 
                    FullName, 
                	CustomerNumber,
                	ZoneTitle,
                	UsageTitle,
                    EventDate as EventDateJalali,
                    Amount,
                	Case When SumAmount>=0 Then N'{ReportLiterals.Due}' Else N'{ReportLiterals.Overdue}' End as AmountState
                FROM OrderedData
                WHERE IsPayed = 1
                ORDER BY 
                	ZoneId,
                	CustomerNumber, 
                	EventDate";
        }
    }
}
