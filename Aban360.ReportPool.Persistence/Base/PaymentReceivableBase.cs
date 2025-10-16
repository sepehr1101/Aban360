using Aban360.ReportPool.Domain.Base;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class PaymentReceivableBase : AbstractBaseConnection
    {
        public PaymentReceivableBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool isWater, bool hasZone, bool hasUsage = false)
        {
            return isWater ? GetWaterDetailQuery(hasZone) : GetServiceLinkDetailQuery(hasZone, hasUsage);
        }

        internal string GetGroupedQuery(bool isWater, bool hasZone, string groupingField, bool hasUsage = false)
        {
            return isWater?GetWaterGroupedQuery(hasZone,hasUsage, groupingField): GetServiceLinkGroupedQuery(hasZone, hasUsage, groupingField);
        }

        private string GetWaterGroupedQuery(bool hasZone, bool hasUsage,string groupingField)
        {
            string queryCondition = GetQueryCondition(hasZone, false);

            return @$"Select 
                        b.{groupingField} as ItemTitle,
                        b.{groupingField},
                    	COUNT(1) as BillCount,
						COUNT(Distinct b.BillId) as CustomerCount,
						SUM(p.Amount) as Amount,
						SUM(Case When p.PayDateJalali<=b.DeadLine Then p.Amount Else 0 End) as DueAmount,
						SUM(Case When p.PayDateJalali>b.DeadLine Then p.Amount Else 0 End ) as OverdueAmount,
						Count(Case When p.PayDateJalali<=b.DeadLine Then 1 End) as DueCount,
						Count(Case When p.PayDateJalali>b.DeadLine Then 1 End ) as OverdueCount
                    From [CustomerWarehouse].dbo.Bills b
                    LEFT JOIN [CustomerWarehouse].dbo.Payments p ON p.BillTableId=b.Id
                    WHERE
                        (@FromDateJalali IS NULL
                     	    OR @ToDateJalali IS NULL 
                     	    OR p.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali)
                        AND (@fromBankId IS NULL OR
						    @toBankId IS NULL OR
						    p.BankCode BETWEEN @fromBankId AND @toBankId)
                        {queryCondition}
						Group By b.{groupingField}";
        }

        private string GetServiceLinkGroupedQuery(bool hasZone, bool hasUsage, string groupingField)
        {
            string queryCondition = GetQueryCondition(hasZone, hasUsage);

            return @$"WITH OrderedData AS (
                    SELECT 
                        p.FullName, 
                		p.CustomerNumber,
                		p.ZoneId,
                		p.ZoneTitle,
                		p.UsageId,
                		p.UsageTitle,
                        p.EventDate,
                        p.IsPayed,
                        p.Amount,
                        SUM(p.Amount) OVER (
                            PARTITION BY p.ZoneId,p.CustomerNumber
                            ORDER BY EventDate
                            ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
                        ) AS SumAmount
                	from [CustomerWarehouse].dbo.PaymentDue p
                	where 
                		 (@FromDateJalali IS NULL OR
                        @ToDateJalali IS NULL OR
                        p.EventDate BETWEEN @FromDateJalali and @ToDateJalali) 
                        {queryCondition}
                )
                SELECT 
                	{groupingField} as ItemTitle,
                	{groupingField} ,
                	SUM(Case When SumAmount>=0 Then SumAmount else 0 end) as OverDueAmount,
                	SUM(Case When SumAmount<0 Then SumAmount else 0 end) as DueAmount,
                	Count(Case When SumAmount>=0 Then 1 else null end) as OverDueCount,
                	Count(Case When SumAmount<0 Then 1 else null end) as DueCount
                FROM OrderedData
                WHERE  IsPayed=1
                Group By {groupingField}";
        }
        private string GetWaterDetailQuery(bool hasZone)
        {
            string queryCondition = GetQueryCondition(hasZone, false);

            return @$"Select 
                    	b.BillId,
                        b.CustomerNumber,
                        '' as FullName,
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
                        (@FromDateJalali IS NULL
                     	    OR @ToDateJalali IS NULL 
                     	    OR p.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali)
                        AND (@fromBankId IS NULL OR
						    @toBankId IS NULL OR
						    p.BankCode BETWEEN @fromBankId AND @toBankId)
                         {queryCondition}";
        }
        
        private string GetServiceLinkDetailQuery(bool hasZone, bool hasUsage)
        {
            string queryCondition = GetQueryCondition(hasZone, hasUsage);

            return @$"WITH OrderedData AS (
                    SELECT 
                        p.FullName, 
                		p.CustomerNumber,
                		p.ZoneId,
                		p.ZoneTitle,
                		p.UsageId,
                		p.UsageTitle,
                        p.EventDate,
                        p.IsPayed,
                        p.Amount,
                        SUM(p.Amount) OVER (
                            PARTITION BY p.ZoneId,p.CustomerNumber
                            ORDER BY EventDate
                            ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
                        ) AS SumAmount
                	from [CustomerWarehouse].dbo.PaymentDue p
                    Where 
                        (@FromDateJalali IS NULL OR
                        @ToDateJalali IS NULL OR
                        p.EventDate BETWEEN @FromDateJalali and @ToDateJalali) 
                        {queryCondition}
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
    
        private string GetQueryCondition(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? " AND p.ZoneId IN @ZoneIds" : string.Empty;
            string usageQuery = hasUsage ? " AND p.UsageId IN @UsageIds" : string.Empty;

            return zoneQuery + usageQuery;
        }
    }
}
