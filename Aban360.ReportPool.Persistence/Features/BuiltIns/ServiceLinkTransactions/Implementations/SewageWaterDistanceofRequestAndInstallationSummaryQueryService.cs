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
    internal sealed class SewageWaterDistanceofRequestAndInstallationSummaryQueryService : SewageWaterDistanceofRequestAndInstallationBase, ISewageWaterDistanceofRequestAndInstallationSummaryQueryService
    {
        public SewageWaterDistanceofRequestAndInstallationSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input)
        {
            string query = GetGroupedQuery(input.IsWater, input.IsInstallation, GroupingFields.UsageTitle);
            string reportTitle = GetTitle(input.IsWater, input.IsInstallation);

            IEnumerable<SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>(query, input);
            SewageWaterDistanceofRequestAndInstallationHeaderOutputDto RequestHeader = new SewageWaterDistanceofRequestAndInstallationHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                Title = reportTitle
            };
            var result = new ReportOutput<SewageWaterDistanceofRequestAndInstallationHeaderOutputDto, SewageWaterDistanceofRequestAndInstallationSummaryDataOutputDto>
                (reportTitle,
                RequestHeader,
                RequestData);

            return result;
        }
        private string GetTitle(bool IsWater, bool IsInstallation)
        {
            if (IsWater)
            {
                return IsInstallation ? ReportLiterals.WaterDistanceInstallationRegisterSummary + ReportLiterals.ByUsage : ReportLiterals.WaterDistanceRequestRegisterSummary + ReportLiterals.ByUsage;
            }
            else
            {
                return IsInstallation ? ReportLiterals.SewageDistanceInstallationeRegisterSummary + ReportLiterals.ByUsage : ReportLiterals.SewageDistanceRequesteRegisterSummary + ReportLiterals.ByUsage;
            }
        }
    }
}
