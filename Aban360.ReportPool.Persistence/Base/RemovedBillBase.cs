using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Base
{
    internal abstract class RemovedBillBase : AbstractBaseConnection
    {
        public RemovedBillBase(IConfiguration configuration)
            : base(configuration)
        { }

        internal string GetDetailQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @zoneIds" : string.Empty;
            return @$"Select
                	c.ZoneTitle,
                	c.ZoneId,
                	c.BillId,
                	rb.PreviousNumber AS PreviousMeterNumber,
                	rb.NextNumber CurrentMeterNumber,
                	rb.NextDay AS CurrentDateJalali,
                	rb.PreviousDay AS PreviousDateJalali,
                	rb.Consumption,
                	rb.SumItems AS Amount,
                	rb.RegisterDay AS RemovedDateJalali,
                	TRIM(c.FirstName) AS FirstName,
                	TRIM(c.SureName) AS Surname,
                	(TRIM(c.FirstName)+' ' +TRIM(c.SureName)) AS FullName,
                	TRIM(c.MobileNo) AS MobileNumber,
                	TRIM(c.NationalId) AS NationalCode,
                	TRIM(c.PostalCode) AS PostalCode,
                	c.UsageTitle
                From [CustomerWarehouse].dbo.RemovedBills rb
                Join [CustomerWarehouse].dbo.Clients c
                	on c.CustomerNumber=rb.CustomerNumber AND c.ZoneId=rb.ZoneId
                Where
                	(@FromDateJalali IS NULL OR
                	@ToDateJalali IS NULL OR
                	rb.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali) AND
                	(@fromAmount IS NULL OR
                	@toAmount IS NULL OR
                	rb.SumItems BETWEEN @fromAmount AND @toAmount) AND
                    (@fromReadingNumber IS NULL OR
                    @toReadingNumber IS NULL OR
                    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
					c.ToDayJalali IS NULL
                    {zoneQuery}";
        }

        internal string GetGroupedQuery(bool hasZone,string groupingField)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @zoneIds" : string.Empty;
            return @$"Select
						MAX(t46.C2) AS RegionTitle,
                      	c.{groupingField} AS ItemTitle,
                      	c.{groupingField},
                        Count(c.{groupingField}) As CustomerCount,
                      	AVG(rb.Consumption) AS AverageConsumption,
                      	SUM(rb.Consumption) AS SumConsumption,
                      	SUM(rb.SumItems) AS Amount
                      From [CustomerWarehouse].dbo.RemovedBills rb
                      Join [CustomerWarehouse].dbo.Clients c
                      	on c.CustomerNumber=rb.CustomerNumber AND c.ZoneId=rb.ZoneId
                      Join [Db70].dbo.T51 t51
				        	On t51.C0=c.ZoneId
				        Join [Db70].dbo.T46 t46
				        	On t51.C1=t46.C0
                      Where
                      	(@FromDateJalali IS NULL OR
                      	@ToDateJalali IS NULL OR
                      	rb.RegisterDay BETWEEN @FromDateJalali AND @ToDateJalali) AND
                      	(@fromAmount IS NULL OR
                      	@toAmount IS NULL OR
                      	rb.SumItems BETWEEN @fromAmount AND @toAmount) AND
                        (@fromReadingNumber IS NULL OR
                        @toReadingNumber IS NULL OR
                        c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
				       	c.ToDayJalali IS NULL
                        {zoneQuery}
                      Group By c.{groupingField}";
        }
    }
}
