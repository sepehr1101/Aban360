using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.CustomersTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.CustomersTransactions.Implementations
{
    internal sealed class NonPermanentBranchSummaryByZoneQueryService : AbstractBaseConnection, INonPermanentBranchSummaryByZoneQueryService
    {
        public NonPermanentBranchSummaryByZoneQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }
        public async Task<ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByZoneDataOutputDto>> GetInfo(NonPermanentBranchByUsageAndZoneInputDto input)
        {
            string nonPremanentBranchQuery = GetNonPermanentBranchQuery();
            var @params = new
            {
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,

                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,

                zoneIds = input.ZoneIds,
                input.UsageIds,
            };

            IEnumerable<NonPermanentBranchSummaryByZoneDataOutputDto> nonPremanentBranchData = await _sqlReportConnection.QueryAsync<NonPermanentBranchSummaryByZoneDataOutputDto>(nonPremanentBranchQuery, @params);
            NonPermanentBranchHeaderOutputDto nonPremanentBranchHeader = new NonPermanentBranchHeaderOutputDto()
            {
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = nonPremanentBranchData is not null && nonPremanentBranchData.Any() ? nonPremanentBranchData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),

                SumCommercialUnit = nonPremanentBranchData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = nonPremanentBranchData.Sum(i => i.DomesticUnit),
                SumOtherUnit = nonPremanentBranchData.Sum(i => i.OtherUnit),
                TotalUnit = nonPremanentBranchData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<NonPermanentBranchHeaderOutputDto, NonPermanentBranchSummaryByZoneDataOutputDto>(ReportLiterals.NonPermanentBranchSummary + ReportLiterals.ByZone, nonPremanentBranchHeader, nonPremanentBranchData);

            return result;
        }

        private string GetNonPermanentBranchQuery()
        {
            return @"SELECT 
						c.ZoneTitle,
						COUNT(c.ZoneTitle) AS CustomerCount,
					    SUM(ISNULL(c.CommercialCount, 0) + ISNULL(c.DomesticCount, 0) + ISNULL(c.OtherCount, 0)) AS TotalUnit,
					    SUM(ISNULL(c.CommercialCount, 0)) AS CommercialUnit,
                        SUM(ISNULL(c.DomesticCount, 0)) AS DomesticUnit,
                        SUM(ISNULL(c.OtherCount, 0)) AS OtherUnit,
						SUM(CASE WHEN t5.C0 = 0 THEN 1 ELSE 0 END) AS UnSpecified,
						SUM(CASE WHEN t5.C0 = 1 THEN 1 ELSE 0 END) AS Field0_5,
						SUM(CASE WHEN t5.C0 = 2 THEN 1 ELSE 0 END) AS Field0_75,
						SUM(CASE WHEN t5.C0 = 3 THEN 1 ELSE 0 END) AS Field1,
						SUM(CASE WHEN t5.C0 = 4 THEN 1 ELSE 0 END) AS Field1_2,
						SUM(CASE WHEN t5.C0 = 5 THEN 1 ELSE 0 END) AS Field1_5,
						SUM(CASE WHEN t5.C0 = 6 THEN 1 ELSE 0 END) AS Field2,
						SUM(CASE WHEN t5.C0 = 7 THEN 1 ELSE 0 END) AS Field3,
						SUM(CASE WHEN t5.C0 = 8 THEN 1 ELSE 0 END) AS Field4,
						SUM(CASE WHEN t5.C0 = 9 THEN 1 ELSE 0 END) AS Field5,
						SUM(CASE WHEN t5.C0 In (10,11,12,13,15) THEN 1 ELSE 0 END) AS MoreThan6
                    FROM [CustomerWarehouse].dbo.Clients c
					Join [Db70].dbo.T5 t5
						On t5.C0=c.WaterDiameterId
                    WHERE 
            			c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						 @toReadingNumber IS NULL OR
						 c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
						 (c.RegisterDayJalali BETWEEN @fromDate AND @toDate) AND
						c.ZoneId in @zoneIds AND
                        c.UsageId in @usageIds AND
						c.IsNonPermanent=1
					Group By 
						c.ZoneTitle";
        }
    }
}
