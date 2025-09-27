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
    internal sealed class SewageWaterInstallationSummaryQueryService : RequestOrInstallBase, ISewageWaterInstallationSummaryQueryService
    {
        public SewageWaterInstallationSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>> Get(SewageWaterInstallationInputDto input)
        {
            string UsageTitle = nameof(UsageTitle);
            string reportTitle = input.IsWater ? ReportLiterals.WaterInstallationSummary : ReportLiterals.SewageInstallationSummary;
            string query= GetGroupedQuery(input.IsWater, false, UsageTitle);            
            var @params = new
            {
                fromDate = input.FromDateJalali,
                toDate = input.ToDateJalali,
                zoneIds = input.ZoneIds,
                usageIds=input.UsageIds,
                fromReadingNumber = input.FromReadingNumber,
                toReadingNumber = input.ToReadingNumber,
            };
            IEnumerable<SewageWaterInstallationSummaryDataOutputDto> installationData = await _sqlReportConnection.QueryAsync<SewageWaterInstallationSummaryDataOutputDto>(query, @params);
            SewageWaterInstallationHeaderOutputDto installationHeader = new SewageWaterInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (installationData is not null && installationData.Any()) ? installationData.Count() : 0,
                Title=reportTitle,

                CustomerCount = installationData.Sum(i => i.CustomerCount),
                SumCommercialUnit = installationData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = installationData.Sum(i => i.DomesticUnit),
                SumOtherUnit = installationData.Sum(i => i.OtherUnit),
                TotalUnit = installationData.Sum(i => i.TotalUnit),
            };
            var result = new ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>
                (reportTitle, installationHeader, installationData);
            
            return result;
        }
    }
}
