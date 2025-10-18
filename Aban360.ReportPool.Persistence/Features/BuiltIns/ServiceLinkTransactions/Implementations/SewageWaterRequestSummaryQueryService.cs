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
    internal sealed class SewageWaterRequestSummaryQueryService : RequestOrInstallBase, ISewageWaterRequestSummaryQueryService
    {
        public SewageWaterRequestSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>> Get(SewageWaterRequestInputDto input)
        {
            string UsageTitle = nameof(UsageTitle);
            string query = GetGroupedQuery(input.IsWater, InstallOrRequestOrInstallDepartmentEnum.Request, UsageTitle);
            string reportTitle = input.IsWater ? ReportLiterals.WaterRequestSummary + ReportLiterals.ByUsage : ReportLiterals.SewageRequestSummary + ReportLiterals.ByUsage;

            IEnumerable<SewageWaterRequestSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterRequestSummaryDataOutputDto>(query, input);
            SewageWaterRequestHeaderOutputDto RequestHeader = new SewageWaterRequestHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = RequestData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = RequestData.Sum(i => i.DomesticUnit),
                SumOtherUnit = RequestData.Sum(i => i.OtherUnit),
                TotalUnit = RequestData.Sum(i => i.TotalUnit),
                CustomerCount = RequestData.Sum(i => i.CustomerCount),
            };
            var result = new ReportOutput<SewageWaterRequestHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>
                (reportTitle,
                RequestHeader,
                RequestData);

            return result;
        }
    }
}
