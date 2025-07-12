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
    internal sealed class SewageWaterRequestSummaryQueryService : AbstractBaseConnection, ISewageWaterRequestSummaryQueryService
    {
        public SewageWaterRequestSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>> Get(SewageWaterRequestInputDto input)
        {
            string RequestSummaryQuery;
            if (input.IsWater)
                RequestSummaryQuery = GetWaterRequestSummaryQuery();
            else
                RequestSummaryQuery = GetSewageRequestSummaryQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterRequestSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterRequestSummaryDataOutputDto>(RequestSummaryQuery, @params);
            SewageWaterRequestHeaderOutputDto RequestHeader = new SewageWaterRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0
            };
            var result = new ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterRequestSummary : ReportLiterals.SewageRequestSummary,
                RequestHeader,
                RequestData);

            return result;
        }
        private string GetWaterRequestSummaryQuery()
        {
            return @"Select	
                    	c.UsageTitle2 AS UsageTitle,
                    	COUNT(c.UsageTitle2) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterRequestDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds
                    Group BY
                    	c.UsageTitle2";
        }
        private string GetSewageRequestSummaryQuery()
        {
            return @"Select	
                    	c.UsageTitle2 AS UsageTitle,
                    	COUNT(c.UsageTitle2) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.SewageRequestDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds
                    Group BY
                    	c.UsageTitle2";
        }
    }
}
