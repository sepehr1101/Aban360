using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ReadingIssueDistanceBillSummaryByUsageQueryService : AbstractBaseConnection, IReadingIssueDistanceBillSummaryByUsageQueryService
    {
        public ReadingIssueDistanceBillSummaryByUsageQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>> GetInfo(ReadingIssueDistanceBillInputDto input)
        {
            string readingIssueDistanceQueryString = GetReadingIssueDistanceDataQuery();
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
            };

            IEnumerable<ReadingIssueDistanceBillSummryDataOutputDto> readingIssueDistanceData = await _sqlReportConnection.QueryAsync<ReadingIssueDistanceBillSummryDataOutputDto>(readingIssueDistanceQueryString, @params);
            ReadingIssueDistanceBillHeaderOutputDto readingIssueDistanceHeader = new ReadingIssueDistanceBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                RecordCount = readingIssueDistanceData is not null && readingIssueDistanceData.Any() ? readingIssueDistanceData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = readingIssueDistanceData is not null && readingIssueDistanceData.Any() ? readingIssueDistanceData.Count() : 0,

                SumCommercialUnit = readingIssueDistanceData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = readingIssueDistanceData.Sum(i => i.DomesticUnit),
                SumOtherUnit = readingIssueDistanceData.Sum(i => i.OtherUnit),
                TotalUnit = readingIssueDistanceData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillSummryDataOutputDto>(ReportLiterals.ReadingIssueDistanceBillSummary + ReportLiterals.ByUsage, readingIssueDistanceHeader, readingIssueDistanceData);

            return result;
        }

        private string GetReadingIssueDistanceDataQuery()
        {
            return @"Select
                      	b.UsageTitle as ItemTitle,
                      	COUNT(b.UsageTitle) as CustomerCount,
                      	SUM(ISNULL(b.CommercialCount, 0) + ISNULL(b.DomesticCount, 0) + ISNULL(b.OtherCount, 0)) AS TotalUnit,
                      	SUM(ISNULL(b.CommercialCount, 0)) AS CommercialUnit,
                      	SUM(ISNULL(b.DomesticCount, 0)) AS DomesticUnit,
                      	SUM(ISNULL(b.OtherCount, 0)) AS OtherUnit,
                      	AVG(CASE WHEN t5.C0 = 0 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS UnSpecified,
                      	AVG(CASE WHEN t5.C0 = 1 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field0_5,
                      	AVG(CASE WHEN t5.C0 = 2 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field0_75,
                      	AVG(CASE WHEN t5.C0 = 3 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field1,
                      	AVG(CASE WHEN t5.C0 = 4 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field1_2,
                      	AVG(CASE WHEN t5.C0 = 5 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field1_5,
                      	AVG(CASE WHEN t5.C0 = 6 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field2,
                      	AVG(CASE WHEN t5.C0 = 7 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field3,
                      	AVG(CASE WHEN t5.C0 = 8 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field4,
                      	AVG(CASE WHEN t5.C0 = 9 THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS Field5,
                      	AVG(CASE WHEN t5.C0 In (10,11,12,13,15) THEN CONVERT(float, DATEDIFF(DAY, [CustomerWarehouse].dbo.PersianToMiladi(b.NextDay), [CustomerWarehouse].dbo.PersianToMiladi(b.RegisterDay))) ELSE null END) AS MoreThan6
                      From [CustomerWarehouse].dbo.Bills b
                      Join [Db70].dbo.T5 t5
                      	On t5.C0=b.WaterDiameterId
                      Where 
                      	b.TypeCode=1 AND
                      	(@fromDate IS NULL OR
                      	@toDate IS NULL OR
                      	b.RegisterDay BETWEEN @fromDate AND @toDate) AND
                      	(@fromReadingNumber IS NULL OR
                      	@toReadingNumber IS NULL OR
                      	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                      	b.ZoneId IN @zoneIds
                     Group by b.UsageTitle";
        }

    }
}
