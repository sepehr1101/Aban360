using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Constants;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class WaterUsageGroupedQueryService : PaymentBase, IWaterUsageGroupedQueryService
    {
        public WaterUsageGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>> GetInfo(ServiceLinkWaterItemGroupedInputDto input)
        {
            string query = GetGroupedQuery(true, input.ZoneIds.HasValue(), GroupingFields.UsageTitle);

            IEnumerable<ServiceLinkWaterItemGroupedDataOutputDto> waterUsageGroupedData = await _sqlReportConnection.QueryAsync<ServiceLinkWaterItemGroupedDataOutputDto>(query, input);
            ServiceLinkWaterItemGroupedHeaderOutputDto waterUsageGroupedHeader = new ServiceLinkWaterItemGroupedHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromBankId=input.FromBankId,
                ToBankId=input.ToBankId,
                RecordCount = (waterUsageGroupedData is not null && waterUsageGroupedData.Any()) ? waterUsageGroupedData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                TotalAmount = waterUsageGroupedData.Sum(usage => usage.Amount),
                Title= ReportLiterals.WaterUsageGrouped,

                CustomerCount = waterUsageGroupedData is not null && waterUsageGroupedData.Any() ? waterUsageGroupedData.Count() : 0,
                SumCommercialUnit = waterUsageGroupedData?.Sum(i => i.CommercialUnit) ?? 0,
                SumDomesticUnit = waterUsageGroupedData?.Sum(i => i.DomesticUnit) ?? 0,
                SumOtherUnit = waterUsageGroupedData?.Sum(i => i.OtherUnit) ?? 0,
                TotalUnit = waterUsageGroupedData?.Sum(i => i.TotalUnit) ?? 0
            };

            var result = new ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>(ReportLiterals.WaterUsageGrouped, waterUsageGroupedHeader, waterUsageGroupedData);
            return result;
        }
    }
}
