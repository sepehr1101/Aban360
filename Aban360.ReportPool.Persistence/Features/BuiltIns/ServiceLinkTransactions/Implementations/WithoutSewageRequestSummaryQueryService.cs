using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class WithoutSewageRequestSummaryQueryService : WithoutSewageRequestBase, IWithoutSewageRequestSummaryQueryService
    {
        public WithoutSewageRequestSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>> Get(WithoutSewageRequestInputDto input)
        {
            string query = GetGroupedQuery(GroupingFields.UsageTitle);
            //string query = GetBranchWithoutSewageRequestQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
                zoneIds = input.ZoneIds
            };
            IEnumerable<WithoutSewageRequestSummaryDataOutputDto> withoutSewageRequestData = await _sqlReportConnection.QueryAsync<WithoutSewageRequestSummaryDataOutputDto>(query, @params);
            WithoutSewageRequestHeaderOutputDto withoutSewageRequestHeader = new WithoutSewageRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (withoutSewageRequestData is not null && withoutSewageRequestData.Any()) ? withoutSewageRequestData.Count() : 0,

                SumCommercialUnit = withoutSewageRequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = withoutSewageRequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = withoutSewageRequestData.Sum(i => i.OtherUnit),
                TotalUnit = withoutSewageRequestData.Sum(i => i.TotalUnit),
                CustomerCount = withoutSewageRequestData.Sum(i => i.CustomerCount),
            };
            var result = new ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>
                (ReportLiterals.WithoutSewageRequestSummary, withoutSewageRequestHeader, withoutSewageRequestData);

            return result;
        }
        private string GetBranchWithoutSewageRequestQuery()
        {
            return @"Select	
                    	c.UsageTitle AS UsageTitle,
                        COUNT(c.UsageTitle) AS CustomerCount,
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
                    From [CustomerWarehouse].dbo.Clients c
					Join [Db70].dbo.T5 t5
						On t5.C0=c.WaterDiameterId
                    Where	
                    	c.WaterRegisterDateJalali BETWEEN @fromDate AND @toDate AND
						(TRIM(c.SewageRequestDate)='' OR c.SewageRequestDate IS NULL) AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL AND
						(@fromReadingNumber IS NULL OR
						@toReadingNumber IS NULL OR
						c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)
                    Group BY
                    	c.UsageTitle";
        }
    }
}
