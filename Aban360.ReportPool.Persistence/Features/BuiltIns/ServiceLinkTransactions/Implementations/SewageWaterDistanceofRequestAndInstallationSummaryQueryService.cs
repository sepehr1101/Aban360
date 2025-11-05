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

        public async Task<ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto>> Get(SewageWaterDistanceofRequestAndInstallationInputDto input)
        {
            string query = GetGroupedQuery(input.IsWater, input.IsInstallation, GroupingFields.UsageTitle);
            string reportTitle = GetTitle(input.IsWater, input.IsInstallation);

            IEnumerable<SewageWaterDistanceSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterDistanceSummaryDataOutputDto>(query, input);
            SewageWaterDistanceHeaderOutputDto RequestHeader = new SewageWaterDistanceHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                CustomerCount = RequestData?.Sum(s=>s.CustomerCount)??0,
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = RequestData?.Sum(r => r.CommercialUnit) ?? 0,
                SumDomesticUnit = RequestData?.Sum(r => r.DomesticUnit) ?? 0,
                SumOtherUnit = RequestData?.Sum(r => r.OtherUnit) ?? 0,
                TotalUnit = RequestData?.Sum(r => r.TotalUnit) ?? 0,
            };
            var result = new ReportOutput<SewageWaterDistanceHeaderOutputDto, SewageWaterDistanceSummaryDataOutputDto>
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
