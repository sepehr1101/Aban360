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
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (installationData is not null && installationData.Any()) ? installationData.Count() : 0
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
                    	c.UsageTitle AS UsageTitle,
                    	COUNT(c.UsageTitle) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.WaterInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                        (@fromReadingNumber IS NULL OR
					    @toReadingNumber IS NULL OR
					    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)
                    Group BY
                    	c.UsageTitle";
        }
        private string GetSewageInstallationSummaryQuery()
        {
            return @"Select	
                    	c.UsageTitle AS UsageTitle,
                    	COUNT(c.UsageTitle) AS Count
                    From [CustomerWarehouse].dbo.Clients c
                    Where	
                    	c.SewageInstallDate BETWEEN @fromDate AND @toDate AND
                    	c.ZoneId IN @zoneIds AND
                        (@fromReadingNumber IS NULL OR
					    @toReadingNumber IS NULL OR
					    c.ReadingNumber BETWEEN @fromReadingNumber AND @toReadingNumber)
                    Group BY
                    	c.UsageTitle";
        }
    }
}
