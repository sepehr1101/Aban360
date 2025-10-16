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
    internal sealed class SewageWaterRequestNonInstalledSummaryQueryService : NonInstalledBase, ISewageWaterRequestNonInstalledSummaryQueryService
    {
        public SewageWaterRequestNonInstalledSummaryQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>> Get(SewageWaterRequestNonInstalledInputDto input)
        {
            string reportTitle = input.IsWater ? ReportLiterals.WaterRequestNonInstalledSummary + ReportLiterals.ByUsage : ReportLiterals.SewageRequestNonInstalledSummary + ReportLiterals.ByUsage;
            string query = GetGroupedQuery(input.IsWater, GroupingFields.UsageTitle);
            
            IEnumerable<SewageWaterRequestNonInstalledSummaryDataOutputDto> requestNonInstalledData = await _sqlReportConnection.QueryAsync<SewageWaterRequestNonInstalledSummaryDataOutputDto>(query, input);
            SewageWaterRequestNonInstalledHeaderOutputDto requestNonInstalledHeader = new SewageWaterRequestNonInstalledHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromReadingNumber = input.FromReadingNumber,
                ToReadingNumber = input.ToReadingNumber,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = (requestNonInstalledData is not null && requestNonInstalledData.Any()) ? requestNonInstalledData.Count() : 0,
                Title=reportTitle,

                SumCommercialUnit = requestNonInstalledData.Sum(i => i.CommercialUnit),
                SumDomesticUnit = requestNonInstalledData.Sum(i => i.DomesticUnit),
                SumOtherUnit = requestNonInstalledData.Sum(i => i.OtherUnit),
                TotalUnit = requestNonInstalledData.Sum(i => i.TotalUnit),
                CustomerCount = requestNonInstalledData.Sum(i => i.CustomerCount),
            };
            var result = new ReportOutput<SewageWaterRequestNonInstalledHeaderOutputDto, SewageWaterRequestNonInstalledSummaryDataOutputDto>
                (reportTitle,
                requestNonInstalledHeader,
                requestNonInstalledData);

            return result;
        }
    }
}
