using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class ReadingStatusStatementBase : AbstractBaseConnection
    {
        public ReadingStatusStatementBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool isRegisterDate)
        {
            QueryParam dataField = GetQueryParam(isRegisterDate);

            return @$"Select 
                    	b.ZoneTitle AS ZoneTitle,
						SUM(b.SumItems) AS SumItems,
                    	b.{dataField.DateField} AS EventDateJalali,
                    	COUNT(Case When b.CounterStateCode NOT IN (1,4,7,8) Then 1 End)AS ReadingNet,
                    	COUNT(Case When b.CounterStateCode=4 Then 1 End)AS Closed,
                    	COUNT(Case When b.CounterStateCode=7 Then 1 End)AS Obstacle,
                    	COUNT(Case When b.CounterStateCode=8 Then 1 End)AS Temporarily,
                    	COUNT(Case When b.CounterStateCode!=1 Then 1 End)AS AllCount,
                    	COUNT(Case When b.CounterStateCode=1 Then 1 End)AS Ruined,
						COUNT(Case When b.ReadingStateTitle IN (N'خوداظهاری حضوری',N'خوداظهاری غیرحضوری')Then 1 End) as SelfClaimedCount,
						SUM(w.Debt) as Debt
                    From [CustomerWarehouse].dbo.Bills b	
					Left Join [CustomerWarehouse].dbo.WaterDebt w
						On TRIM(b.BillId) Collate SQL_Latin1_General_CP1_CI_AS=w.BillId
                    Where
                    	(b.{dataField.DateField} BETWEEN @fromDate AND @toDate)AND
                        (@FromReadingNumber IS NULL or
                    	@ToReadingNumber IS NULL or 
                    	b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	b.ZoneId IN @zoneIds
                    Group By 
                    	b.{dataField.DateField},
                        b.ZoneTitle";
        }

        internal string GetGroupedQuery(bool isRegisterDate, bool isZone)
        {
            QueryParam dataField = GetQueryParam(isRegisterDate, isZone);

            return @$"Select 
						MAX(t46.C2) AS RegionTitle,
                    	b.{dataField.GroupedField} AS {dataField.GroupedField},
                    	b.{dataField.GroupedField} AS ItemTitle ,
						SUM(b.SumItems) AS SumItems,
                    	COUNT(Case When b.CounterStateCode NOT IN (1,4,7,8) Then 1 End)AS ReadingNet,
                    	COUNT(Case When b.CounterStateCode=4 Then 1 End)AS Closed,
                    	COUNT(Case When b.CounterStateCode=7 Then 1 End)AS Obstacle,
                    	COUNT(Case When b.CounterStateCode=8 Then 1 End)AS Temporarily,
                    	COUNT(Case When b.CounterStateCode!=1 Then 1 End)AS AllCount,
						COUNT(Case When b.ReadingStateTitle IN (N'خوداظهاری حضوری',N'خوداظهاری غیرحضوری')Then 1 End) as SelfClaimedCount,
                    	COUNT(Case When b.CounterStateCode=1 Then 1 End)AS Ruined,
						SUM(w.Debt) as Debt
                    From [CustomerWarehouse].dbo.Bills b	
					Join [Db70].dbo.T51 t51
						On t51.C0=b.ZoneId
					Join [Db70].dbo.T46 t46
						On t51.C1=t46.C0
					Left Join [CustomerWarehouse].dbo.WaterDebt w
						On TRIM(b.BillId) Collate SQL_Latin1_General_CP1_CI_AS=w.BillId
                    Where
                    	(b.{dataField.DateField} BETWEEN @fromDate AND @toDate)AND
                        (@FromReadingNumber IS NULL or
                    	@ToReadingNumber IS NULL or 
                    	b.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	b.ZoneId in @zoneIds
                    Group By b.{dataField.GroupedField}";
        }
        private QueryParam GetQueryParam(bool isRegisterDate, bool isZone = false)
        {
            string RegisterDay = nameof(RegisterDay),
                   NextDay = nameof(NextDay),
                   ZoneTitle = nameof(ZoneTitle),
                   UsageTitle = nameof(UsageTitle);

            string groupedField = isZone ? ZoneTitle : UsageTitle;
            string dateField = isRegisterDate ? RegisterDay : NextDay;

            return new QueryParam(dateField, groupedField);
        }

        private record QueryParam
        {
            public string DateField { get; set; }
            public string GroupedField { get; set; }
            public QueryParam(string dateField, string groupedField)
            {
                DateField = dateField;
                GroupedField = groupedField;
            }
            public QueryParam()
            {
            }
        }
    }
}
