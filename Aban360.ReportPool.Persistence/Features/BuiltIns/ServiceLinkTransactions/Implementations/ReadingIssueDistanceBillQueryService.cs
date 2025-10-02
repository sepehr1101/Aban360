using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class ReadingIssueDistanceBillQueryService : ReadingIssueDistanceBillBase, IReadingIssueDistanceBillQueryService
    {
        public ReadingIssueDistanceBillQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillDataOutputDto>> GetInfo(ReadingIssueDistanceBillInputDto input)
        {
            string query = GetDetailQuery();
            //string query= GetQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds,
            };
            //todo: calc distance in handler
            IEnumerable<ReadingIssueDistanceBillDataOutputDto> readingIssueDistanceData = await _sqlReportConnection.QueryAsync<ReadingIssueDistanceBillDataOutputDto>(query, @params);
            ReadingIssueDistanceBillHeaderOutputDto readingIssueDistanceHeader = new ReadingIssueDistanceBillHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = readingIssueDistanceData is not null && readingIssueDistanceData.Any() ? readingIssueDistanceData.Count() : 0,
                CustomerCount = readingIssueDistanceData is not null && readingIssueDistanceData.Any() ? readingIssueDistanceData.Count() : 0,

                SumCommercialUnit = readingIssueDistanceData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = readingIssueDistanceData.Sum(i => i.DomesticUnit),
                SumOtherUnit = readingIssueDistanceData.Sum(i => i.OtherUnit),
                TotalUnit = readingIssueDistanceData.Sum(i => i.TotalUnit)
            };

            var result = new ReportOutput<ReadingIssueDistanceBillHeaderOutputDto, ReadingIssueDistanceBillDataOutputDto>(ReportLiterals.ReadingIssueDistanceBillDetail, readingIssueDistanceHeader, readingIssueDistanceData);

            return result;
        }

        private string GetQuery()
        {
            return @"Select
                	t46.C2 AS RegionTitle,
                	t46.C0 AS RegionId,
                	b.ZoneTitle,
                	b.ZoneId,
                	b.CustomerNumber,
                	b.ReadingNumber,
                	TRIM(b.BillId) as BillId ,
                	b.PreviousDay as PreviousDateJalali,
                	b.NextDay as CurrentDateJalali,
                	b.PreviousNumber,
                	b.NextNumber,
                	b.RegisterDay as RegisterDateJalali,
                	b.CounterStateTitle,
                	b.UsageTitle,
                	b.BranchType,
                	b.WaterDiameterId,
                	b.WaterDiameterTitle,
                	b.ContractCapacity as ContractualCapacity,
                	b.CommercialCount as CommercialUnit,
                	b.DomesticCount as DomesticUnit,
                	b.OtherCount as OtherUnit,
                	(b.CommercialCount+b.DomesticCount +b.OtherCount) as TotalUnit,
                	b.Consumption,
                	b.ConsumptionAverage,
                	b.ReadingStateTitle,
                	TRIM(c.FirstName) as FirstName,
                	TRIM(c.SureName) as Surname,
                	(TRIM(c.FirstName) + ' ' + TRIM(c.SureName)) as FullName,
                	TRIM(c.NationalId) as NationalCode,
                	TRIM(c.PostalCode) as PostalCode,
                	TRIM(c.PhoneNo) as PhoneNumber,
                	TRIM(c.MobileNo) as MobileNumber,
                	TRIM(c.Address) as Address
                From [CustomerWarehouse].dbo.Bills b
                Join [CustomerWarehouse].dbo.Clients c
                	On c.CustomerNumber=b.CustomerNumber AND c.ZoneId=b.ZoneId
                Join [Db70].dbo.T51 t51
                	On t51.C0=b.ZoneId
                Join [Db70].dbo.T46 t46
                	On t51.C1=t46.C0
                Where 
                    c.ToDayJalali IS NULL AND
                	b.TypeCode=1 AND
                	(@fromDate IS NULL OR
                	@toDate IS NULL OR
                	b.RegisterDay BETWEEN @fromDate AND @toDate) AND
                	(@fromReadingNumber IS NULL OR
                	@toReadingNumber IS NULL OR
                	b.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber) AND
                	b.ZoneId IN @zoneIds";
        }
    }
}
