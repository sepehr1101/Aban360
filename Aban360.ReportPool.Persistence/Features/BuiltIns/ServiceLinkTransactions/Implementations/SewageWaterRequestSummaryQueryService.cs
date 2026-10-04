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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.ServiceLinkTransactions.Implementations
{
    internal sealed class SewageWaterRequestSummaryQueryService : RequestOrInstallBase, ISewageWaterRequestSummaryQueryService
    {
        public SewageWaterRequestSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<SewageWaterRequestSummaryHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>> Get(SewageWaterRequestInputDto input)
        {
            string UsageTitle = nameof(UsageTitle);
            string query = GetGroupedQuery(input.IsWater, InstallOrRequestOrInstallDepartmentEnum.Request, false, UsageTitle, null);
            string reportTitle = input.IsWater ? ReportLiterals.WaterRequestSummary + ReportLiterals.ByUsage : ReportLiterals.SewageRequestSummary + ReportLiterals.ByUsage;

            IEnumerable<SewageWaterRequestSummaryDataOutputDto> RequestData = await _sqlReportConnection.QueryAsync<SewageWaterRequestSummaryDataOutputDto>(query, input);
            SewageWaterRequestSummaryHeaderOutputDto RequestHeader = new SewageWaterRequestSummaryHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (RequestData is not null && RequestData.Any()) ? RequestData.Count() : 0,
                Title = reportTitle,

                SumCommercialUnit = RequestData?.Sum(i => i.CommercialUnit) ?? 0,
                SumDomesticUnit = RequestData?.Sum(i => i.DomesticUnit) ?? 0,
                SumOtherUnit = RequestData?.Sum(i => i.OtherUnit) ?? 0,
                TotalUnit = RequestData?.Sum(i => i.TotalUnit) ?? 0,
                CustomerCount = RequestData?.Sum(i => i.CustomerCount) ?? 0,

                UnSpecified = RequestData?.Sum(i => i.UnSpecified) ?? 0,
                Field0_5 = RequestData?.Sum(i => i.Field0_5) ?? 0,
                Field0_75 = RequestData?.Sum(i => i.Field0_75) ?? 0,
                Field1 = RequestData?.Sum(i => i.Field1) ?? 0,
                Field1_2 = RequestData?.Sum(i => i.Field1_2) ?? 0,
                Field1_5 = RequestData?.Sum(i => i.Field1_5) ?? 0,
                Field2 = RequestData?.Sum(i => i.Field2) ?? 0,
                Field3 = RequestData?.Sum(i => i.Field3) ?? 0,
                Field4 = RequestData?.Sum(i => i.Field4) ?? 0,
                Field5 = RequestData?.Sum(i => i.Field5) ?? 0,
                MoreThan6 = RequestData?.Sum(i => i.MoreThan6) ?? 0,
            };
            var result = new ReportOutput<SewageWaterRequestSummaryHeaderOutputDto, SewageWaterRequestSummaryDataOutputDto>
                (reportTitle,
                RequestHeader,
                RequestData);

            return result;
        }
    }
}
