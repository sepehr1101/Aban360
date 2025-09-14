using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.WaterTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.WaterTransactions.Implementations
{
    internal sealed class WithoutBillSummaryByUsageQueryService : AbstractBaseConnection, IWithoutBillSummaryByUsageQueryService
    {
        public WithoutBillSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>> GetInfo(WithoutBillInputDto input)
        {
            string withoutBill = GetWithoutBillQuery(input.ZoneIds?.Any() == true, input.UsageIds.Any());
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                input.FromReadingNumber,
                input.ToReadingNumber,
                input.ZoneIds,
                usageIds = input.UsageIds,
            };

            IEnumerable<WithoutBillSummaryDataOutputDto> withoutBillData = await _sqlReportConnection.QueryAsync<WithoutBillSummaryDataOutputDto>(withoutBill, @params);
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = withoutBillData is not null && withoutBillData.Any() ? withoutBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryDataOutputDto>(ReportLiterals.WithoutBill + ReportLiterals.ByUsage, withoutBillHeader, withoutBillData);
            return result;
        }

        private string GetWithoutBillQuery(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @ZoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND c.UsageId IN @usageIds" : string.Empty;

            return @$"Select	
                       	c.UsageTitle AS ItemTitle,
                    	COUNT(c.UsageTitle) AS CustomerCount,
                    	SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
                    	SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                    	SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                    	SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit
                    From [CustomerWarehouse].dbo.Clients c
                    LEFt JOIN [CustomerWarehouse].dbo.Bills b
                    	on c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
                    where 
                       	 b.Id IS NULL AND
                    	(@FromDate IS NULL or
                       	@ToDate IS NULL or 
                       	c.WaterInstallDate BETWEEN @FromDate and @ToDate)AND 
                    	(@FromReadingNumber IS NULL or
                       	@ToReadingNumber IS NULL or 
                        c.ReadingNumber BETWEEN @FromReadingNumber and @ToReadingNumber) AND
                       	c.DeletionStateId IN (0,2)  AND
                    	c.ToDayJalali IS NULL
                    	{zoneQuery}
                        {usageQuery}
                    Group By c.UsageTitle ";

        }
    }
}
