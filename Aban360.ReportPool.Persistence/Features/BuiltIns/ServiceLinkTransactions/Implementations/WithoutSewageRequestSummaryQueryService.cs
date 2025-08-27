using Aban360.Common.Db.Dapper;
using Aban360.Common.Excel;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class WithoutSewageRequestSummaryQueryService : AbstractBaseConnection, IWithoutSewageRequestSummaryQueryService
    {
        public WithoutSewageRequestSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>> Get(WithoutSewageRequestInputDto input)
        {
            string withoutSewageRequest = GetBranchWithoutSewageRequestQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds
            };
            IEnumerable<WithoutSewageRequestSummaryDataOutputDto> withoutSewageRequestData = await _sqlReportConnection.QueryAsync<WithoutSewageRequestSummaryDataOutputDto>(withoutSewageRequest, @params);
            WithoutSewageRequestHeaderOutputDto withoutSewageRequestHeader = new WithoutSewageRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (withoutSewageRequestData is not null && withoutSewageRequestData.Any()) ? withoutSewageRequestData.Count() : 0,
            };
            var result = new ReportOutput<WithoutSewageRequestHeaderOutputDto, WithoutSewageRequestSummaryDataOutputDto>
                (ReportLiterals.WithoutSewageRequestSummary, withoutSewageRequestHeader, withoutSewageRequestData);

            return result;
        }
        private string GetBranchWithoutSewageRequestQuery()
        {
            return @"Select	
                    	c.UsageTitle AS UsageTitle,
                    	COUNT(c.UsageTitle) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
						(TRIM(c.SewageRequestDate)='' OR c.SewageRequestDate IS NULL) AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL
                    Group BY
                    	c.UsageTitle";
        }
    }
}
