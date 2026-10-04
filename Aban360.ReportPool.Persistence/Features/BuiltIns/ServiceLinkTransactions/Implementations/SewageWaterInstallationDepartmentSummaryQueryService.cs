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
    internal sealed class SewageWaterInstallationDepartmentSummaryQueryService : RequestOrInstallBase, ISewageWaterInstallationDepartmentSummaryQueryService
    {
        public SewageWaterInstallationDepartmentSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<SewageWaterInstallationSummaryHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>> Get(SewageWaterInstallationInputDto input)
        {
            string UsageTitle = nameof(UsageTitle);
            string reportTitle = (input.IsWater ? ReportLiterals.WaterInstallationDepartmentSummary : ReportLiterals.SewageInstallationDepartmentSummary) + ReportLiterals.ByUsage;
            string query = GetGroupedQuery(input.IsWater, InstallOrRequestOrInstallDepartmentEnum.InstallDepartment, false, UsageTitle, null);

            IEnumerable<SewageWaterInstallationSummaryDataOutputDto> installationData = await _sqlReportConnection.QueryAsync<SewageWaterInstallationSummaryDataOutputDto>(query, input);
            SewageWaterInstallationSummaryHeaderOutputDto installationHeader = new SewageWaterInstallationSummaryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (installationData is not null && installationData.Any()) ? installationData.Count() : 0,
                Title = reportTitle,

                CustomerCount = installationData?.Sum(i => i.CustomerCount) ?? 0,
                SumCommercialUnit = installationData?.Sum(i => i.CommercialUnit) ?? 0,
                SumDomesticUnit = installationData?.Sum(i => i.DomesticUnit) ?? 0,
                SumOtherUnit = installationData?.Sum(i => i.OtherUnit) ?? 0,
                TotalUnit = installationData?.Sum(i => i.TotalUnit) ?? 0,

                UnSpecified = installationData?.Sum(i => i.UnSpecified) ?? 0,
                Field0_5 = installationData?.Sum(i => i.Field0_5) ?? 0,
                Field0_75 = installationData?.Sum(i => i.Field0_75) ?? 0,
                Field1 = installationData?.Sum(i => i.Field1) ?? 0,
                Field1_2 = installationData?.Sum(i => i.Field1_2) ?? 0,
                Field1_5 = installationData?.Sum(i => i.Field1_5) ?? 0,
                Field2 = installationData?.Sum(i => i.Field2) ?? 0,
                Field3 = installationData?.Sum(i => i.Field3) ?? 0,
                Field4 = installationData?.Sum(i => i.Field4) ?? 0,
                Field5 = installationData?.Sum(i => i.Field5) ?? 0,
                MoreThan6 = installationData?.Sum(i => i.MoreThan6) ?? 0,
            };
            var result = new ReportOutput<SewageWaterInstallationSummaryHeaderOutputDto, SewageWaterInstallationSummaryDataOutputDto>
                (reportTitle, installationHeader, installationData);

            return result;
        }
    }
}
