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
    internal sealed class WithoutBillSummaryByZoneQueryService : AbstractBaseConnection, IWithoutBillSummaryByZoneQueryService
    {
        public WithoutBillSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryByZoneDataOutputDto>> GetInfo(WithoutBillInputDto input)
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

            IEnumerable<WithoutBillSummaryByZoneDataOutputDto> withoutBillData = await _sqlReportConnection.QueryAsync<WithoutBillSummaryByZoneDataOutputDto>(withoutBill, @params);
            WithoutBillHeaderOutputDto withoutBillHeader = new WithoutBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = withoutBillData is not null && withoutBillData.Any() ? withoutBillData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString()
            };

            var result = new ReportOutput<WithoutBillHeaderOutputDto, WithoutBillSummaryByZoneDataOutputDto>(ReportLiterals.WithoutBill + ReportLiterals.ByZone, withoutBillHeader, withoutBillData);
            return result;
        }

        private string GetWithoutBillQuery(bool hasZone, bool hasUsage)
        {
            string zoneQuery = hasZone ? "AND c.ZoneId IN @ZoneIds" : string.Empty;
            string usageQuery = hasUsage ? "AND c.UsageId IN @usageIds" : string.Empty;

            return @$"Select	
                    	MAX(t46.C2) AS RegionTitle,
                       	c.ZoneTitle,
                    	COUNT(c.ZoneTitle) AS CustomerCount,
                    	SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
                    	SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                    	SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                    	SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit
                    From [CustomerWarehouse].dbo.Clients c
                    LEFt JOIN [CustomerWarehouse].dbo.Bills b
                    	on c.ZoneId=b.ZoneId AND c.CustomerNumber=b.CustomerNumber
                    Join [Db70].dbo.T51 t51
                    	On t51.C0=c.ZoneId
                    Join [Db70].dbo.T46 t46
                    	On t51.C1=t46.C0
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
                    Group By c.ZoneTitle ";

        }
    }
}
