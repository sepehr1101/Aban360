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
    internal sealed class SewageWaterInstallationSummaryQueryService : AbstractBaseConnection, ISewageWaterInstallationSummaryQueryService
    {
        public SewageWaterInstallationSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>> Get(SewageWaterInstallationInputDto input)
        {
            string installationSummaryQuery;
            if (input.IsWater)
                installationSummaryQuery = GetWaterInstallationSummaryQuery();
            else
                installationSummaryQuery = GetSewageInstallationSummaryQuery();

            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds
            };
            IEnumerable<SewageWaterInstallationSummaryDataOutputDto> installationData = await _sqlReportConnection.QueryAsync<SewageWaterInstallationSummaryDataOutputDto>(installationSummaryQuery, @params);
            SewageWaterInstallationHeaderOutputDto installationHeader = new SewageWaterInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDate = DateTime.Now.ToShortPersianDateString(),
                RecordCount = installationData.Count()
            };
            var result = new ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>
                (input.IsWater ? ReportLiterals.WaterInstallationSummary : ReportLiterals.SewageInstallationSummary,
                installationHeader,
                installationData);
            
            return result;
        }
        private string GetWaterInstallationSummaryQuery()
        {
            return @"Select	
                    	c.UsageTitle2 AS UsageTitle,
                    	COUNT(c.UsageTitle2) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds
                    Group BY
                    	c.UsageTitle2";
        }
        private string GetSewageInstallationSummaryQuery()
        {
            return @"Select	
                    	c.UsageTitle2 AS UsageTitle,
                    	COUNT(c.UsageTitle2) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.SewageInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds
                    Group BY
                    	c.UsageTitle2";
        }
    }
}
