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
    internal sealed class SewageWaterRequestNonInstalledSummaryQueryService : AbstractBaseConnection, ISewageWaterRequestNonInstalledSummaryQueryService
    {
        public SewageWaterRequestNonInstalledSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>> Get(SewageWaterRequestNonInstalledInputDto input)
        {
            string requestNonInstalledQuery;
            if (input.IsWater)
                requestNonInstalledQuery = GetWaterRequestNonInstalledQuery();
            else
                requestNonInstalledQuery = GetSewageRequestNonInstalledQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterRequestNonInstalledSummaryDataOutputDto> requestNonInstalledData = await _sqlReportConnection.QueryAsync<SewageWaterRequestNonInstalledSummaryDataOutputDto>(requestNonInstalledQuery, @params);
            SewageWaterRequestNonInstalledHeaderOutputDto requestNonInstalledHeader = new SewageWaterRequestNonInstalledHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (requestNonInstalledData is not null && requestNonInstalledData.Any()) ? requestNonInstalledData.Count() : 0
            };
            var result = new ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterRequestNonInstalledSummary : ReportLiterals.SewageRequestNonInstalledSummary,
                requestNonInstalledHeader,
                requestNonInstalledData);

            return result;
        }
        private string GetWaterRequestNonInstalledQuery()
        {
            return @"Select	
                    	c.UsageTitle2 AS UsageTitle,
                    	COUNT(c.UsageTitle2) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterRequestDate BETWEEN @fromDate AND @toDate AND
						(TRIM(c.WaterInstallDate)='' OR c.WaterInstallDate IS NULL) AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL
                    Group BY
                    	c.UsageTitle2";
        }
        private string GetSewageRequestNonInstalledQuery()
        {
            return @"Select	
                    	c.UsageTitle2 AS UsageTitle,
                    	COUNT(c.UsageTitle2) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.SewageRequestDate BETWEEN @fromDate AND @toDate AND
						(TRIM(c.SewageInstallDate)='' OR c.SewageInstallDate IS NULL) AND
                    	c.ZoneId IN @zoneIds AND
						c.ToDayJalali IS NULL
                    Group BY
                    	c.UsageTitle2";
        }
    }
}
