using Aban360.Common.BaseEntities;
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
    internal sealed class SewageWaterInstallationDepartmentSummaryByZoneIdQueryService : RequestOrInstallBase, ISewageWaterInstallationDepartmentSummaryByZoneIdQueryService
    {
        public SewageWaterInstallationDepartmentSummaryByZoneIdQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDataOutputDto>> Get(SewageWaterInstallationInputDto input)
        {
            string ZoneTitle = nameof(ZoneTitle);
            string query = GetGroupedQuery(input.IsWater, InstallOrRequestOrInstallDepartmentEnum.InstallDepartment, ZoneTitle);
            string reportTitle = (input.IsWater ? ReportLiterals.WaterInstallationDepartmentSummary : ReportLiterals.SewageInstallationDepartmentSummary)+ReportLiterals.ByZone;

            IEnumerable<SewageWaterInstallationSummaryByZoneIdDataOutputDto> installationData = await _sqlReportConnection.QueryAsync<SewageWaterInstallationSummaryByZoneIdDataOutputDto>(query, input);
            SewageWaterInstallationHeaderOutputDto installationHeader = new SewageWaterInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = installationData is not null && installationData.Any() ? installationData.Count() : 0,
                Title = reportTitle,

                CustomerCount = installationData.Sum(i => i.CustomerCount),
                SumCommercialUnit = installationData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = installationData.Sum(i => i.DomesticUnit),
                SumOtherUnit = installationData.Sum(i => i.OtherUnit),
                TotalUnit = installationData.Sum(i => i.TotalUnit),
            };
            var result = new ReportOutput<SewageWaterInstallationHeaderOutputDto, SewageWaterInstallationSummaryByZoneIdDataOutputDto>
                (reportTitle,
                installationHeader,
                installationData);

            return result;
        }
    }
}
