using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WithoutBillSummaryByUsageQueryService : WithoutBillBase, IWithoutBillSummaryByUsageQueryService
    {
        public WithoutBillSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>> GetInfo(WithoutBillInputDto input)
        {
            string query = GetGroupedQuery(input.ZoneIds?.Any() == true, input.UsageIds?.Any() == true, false);
            //string query = GetWithoutBillQuery(input.ZoneIds?.Any() == true, input.UsageIds?.Any() == true);
            
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                input.FromReadingNumber,
                input.ToReadingNumber,
                input.ZoneIds,
                usageIds = input.UsageIds,
            };

            IEnumerable<WithoutBillSummaryDataOutputDto> withoutBillData = await _sqlReportConnection.QueryAsync<WithoutBillSummaryDataOutputDto>(query, @params);
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = withoutBillData is not null && withoutBillData.Any() ? withoutBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumCommercialUnit = withoutBillData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = withoutBillData.Sum(i => i.DomesticUnit),
                SumOtherUnit = withoutBillData.Sum(i => i.OtherUnit),
                TotalUnit = withoutBillData.Sum(i => i.TotalUnit),
                CustomerCount = withoutBillData.Sum(i => i.CustomerCount),
            };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>(ReportLiterals.WithoutBill + ReportLiterals.ByUsage, withoutBillHeader, withoutBillData);
            return result;
        }

        private string GetWithoutBillQuery(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @ZoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND c.UsageId IN @usageIds" : string.Empty;

            return $@";With WithoutBill as (
                    Select 
                    	c.ZoneId,
                    	c.CustomerNumber,
                    	c.UsageTitle,
                    	c.CommercialCount,
                    	c.DomesticCount,
                    	c.OtherCount,
                    	c.WaterDiameterId	
                    From [CustomerWarehouse].dbo.Clients c
                    Where NOT EXISTS(
                    		Select 1
                    		From [CustomerWarehouse].dbo.Bills  b
                    		Where 
                    			c.ZoneId=b.ZoneId AND
                    			c.CustomerNumber=b.CustomerNumber AND
                    			(@FromDate IS NULL or
                    			@ToDate IS NULL or 
                    			b.RegisterDay BETWEEN @FromDate and @ToDate)AND 
                    			 (@FromReadingNumber IS NULL or
                    			  @ToReadingNumber IS NULL or 
                    			  c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                            	c.DeletionStateId IN (0,2)  AND
                    			b.TypeCode = 1 AND
                    			c.ToDayJalali IS NULL
                               {zoneQuery}
                               {usageQuery}
                            )AND
                    	(@FromReadingNumber IS NULL or
                    	 @ToReadingNumber IS NULL or 
                    	 c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                    	c.DeletionStateId IN (0,2)  AND
                    	c.ToDayJalali IS NULL
                         {zoneQuery}
                         {usageQuery}
                    )
                    Select 
                    	w.UsageTitle as ItemTitle,
                    	COUNT(w.UsageTitle) AS CustomerCount,
                    	SUM(ISNULL(w.CommercialCount, 0) + ISNULL(w.DomesticCount, 0) + ISNULL(w.OtherCount, 0)) AS TotalUnit,
                    	SUM(ISNULL(w.CommercialCount, 0)) AS CommercialUnit,
                    	SUM(ISNULL(w.DomesticCount, 0)) AS DomesticUnit,
                    	SUM(ISNULL(w.OtherCount, 0)) AS OtherUnit,
                    	SUM(CASE WHEN w.WaterDiameterId = 0 THEN 1 ELSE 0 END) AS UnSpecified,
                    	SUM(CASE WHEN w.WaterDiameterId = 1 THEN 1 ELSE 0 END) AS Field0_5,
                    	SUM(CASE WHEN w.WaterDiameterId = 2 THEN 1 ELSE 0 END) AS Field0_75,
                    	SUM(CASE WHEN w.WaterDiameterId = 3 THEN 1 ELSE 0 END) AS Field1,
                    	SUM(CASE WHEN w.WaterDiameterId = 4 THEN 1 ELSE 0 END) AS Field1_2,
                    	SUM(CASE WHEN w.WaterDiameterId = 5 THEN 1 ELSE 0 END) AS Field1_5,
                    	SUM(CASE WHEN w.WaterDiameterId = 6 THEN 1 ELSE 0 END) AS Field2,
                    	SUM(CASE WHEN w.WaterDiameterId = 7 THEN 1 ELSE 0 END) AS Field3,
                    	SUM(CASE WHEN w.WaterDiameterId = 8 THEN 1 ELSE 0 END) AS Field4,
                    	SUM(CASE WHEN w.WaterDiameterId = 9 THEN 1 ELSE 0 END) AS Field5,
                    	SUM(CASE WHEN w.WaterDiameterId In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    From WithoutBill w
                    Join [CustomerWarehouse].dbo.Bills b
                    	On w.ZoneId=b.ZoneId AND w.CustomerNumber=b.CustomerNumber
                    Where
                    	b.TypeCode = 1 
                    Group By w.UsageTitle";
     //       return @$"Select	
     //                  	c.UsageTitle AS ItemTitle,
     //               	COUNT(c.UsageTitle) AS CustomerCount,
     //               	SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
     //               	SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
     //               	SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
     //               	SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
					//	SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
				 //       SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
				 //       SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
				 //       SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
				 //       SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
				 //       SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
				 //       SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
				 //       SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
				 //       SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
				 //       SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
				 //       SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
     //               From [CustomerWarehouse].dbo.Clients c
     //               LEFt JOIN [CustomerWarehouse].dbo.Bills b
     //               	on c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
					//Join [Db70].dbo.T5 t5
					//	On t5.C0=c.WaterDiameterId
     //               where 
     //                  	 b.Id IS NULL AND
     //               	(@FromDate IS NULL or
     //                  	@ToDate IS NULL or 
     //                  	c.WaterInstallDate BETWEEN @FromDate and @ToDate)AND 
     //               	(@FromReadingNumber IS NULL or
     //                  	@ToReadingNumber IS NULL or 
     //                   c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
     //                  	c.DeletionStateId IN (0,2)  AND
     //               	c.ToDayJalali IS NULL
     //               	{zoneQuery}
     //                   {usageQuery}
     //               Group By c.UsageTitle ";

        }
    }
}
