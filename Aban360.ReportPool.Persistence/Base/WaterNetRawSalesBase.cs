using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class WaterNetRawSalesBase : AbstractBaseConnection
    {
        public WaterNetRawSalesBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool isNet)
        {
            string queryParam = GetQueryParam(isNet);

            return @$"Select 
                    	b.UsageTitle,
                    	b.ZoneTitle,
                    	TRIM(b.BillId) as BillId,
                    	b.Payable,
                    	b.CustomerNumber,
                    	b.ReadingNumber
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.TypeCode IN {queryParam} AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.ZoneId IN @zoneIds";
        }

        internal string GetGroupedQuery(bool isNet)
        {
            string queryParam = GetQueryParam(isNet);

            return @$"Select 
                    	b.UsageTitle,
                    	b.ZoneTitle,
                    	SUM(b.Payable) AS Payable,
                    	COUNT(1) AS Count
                    From [CustomerWarehouse].dbo.Bills b
                    Where 
                    	b.TypeCode IN {queryParam} AND
                    	b.RegisterDay BETWEEN @fromDate AND @toDate AND
                    	b.ZoneId IN @zoneIds 
                    Group By
                    	b.ZoneTitle,
                    	b.UsageTitle";
        }

        private string GetQueryParam(bool isNet)
        {
            string netQuery = "(3,4,5,9)";
            string rawQuery = "(1)";

            return isNet ? netQuery : rawQuery;
        }
    }
}
