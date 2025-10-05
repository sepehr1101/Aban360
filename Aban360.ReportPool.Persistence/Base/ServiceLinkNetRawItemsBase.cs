using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class ServiceLinkNetRawItemsBase : AbstractBaseConnection
    {
        public ServiceLinkNetRawItemsBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(string netRawCondition)
        {
            return @$"Select
                    	r.TrackNumber,
                    	r.ZoneTitle,
                    	r.CustomerNumber,
                    	r.ItemTitle,
                    	r.Amount,
                    	r.OffAmount,
                    	r.FinalAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where	
                    	r.RegisterDate BETWEEN @fromDate AND @toDate AND
                    	r.ZoneId IN @zoneIds 
                        {netRawCondition}";

        }

        internal string GetGroupedQuery(string netRawCondition)
        {
            return $@"Select
                    	r.ItemTitle ,
                    	SUM(r.Amount) AS Amount,
                    	SUM(r.OffAmount) AS OffAmount,
                    	SUM(r.FinalAmount) AS FinalAmount
                    From [CustomerWarehouse].dbo.RequestBillDetails r
                    Where	
                    	r.RegisterDate BETWEEN @fromDate AND @toDate AND
                    	r.ZoneId IN @zoneIds 
                        {netRawCondition}
                    Group By r.ItemTitle";
        }
    }
}
