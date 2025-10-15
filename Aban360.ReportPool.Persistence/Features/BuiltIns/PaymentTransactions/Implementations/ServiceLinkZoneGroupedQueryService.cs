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
    internal sealed class ServiceLinkZoneGroupedQueryService : PaymentBase, IServiceLinkZoneGroupedQueryService
    {
        public ServiceLinkZoneGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>> GetInfo(ServiceLinkWaterItemGroupedInputDto input)
        {
            string query = GetGroupedQuery(false, input.ZoneIds.HasValue(), GroupingFields.ZoneTitle);

            IEnumerable<ServiceLinkWaterItemGroupedDataOutputDto> serviceLinkZoneGroupedData = await _sqlReportConnection.QueryAsync<ServiceLinkWaterItemGroupedDataOutputDto>(query, input);
            ServiceLinkWaterItemGroupedHeaderOutputDto serviceLinkZoneGroupedHeader = new ServiceLinkWaterItemGroupedHeaderOutputDto()
            {
                FromBankId = input.FromBankId,
                ToBankId = input.ToBankId,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = serviceLinkZoneGroupedData is not null && serviceLinkZoneGroupedData.Any() ? serviceLinkZoneGroupedData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                TotalAmount = serviceLinkZoneGroupedData.Sum(usage => usage.Amount),
                Title= ReportLiterals.ServiceLinkZoneGrouped,

                CustomerCount = serviceLinkZoneGroupedData is not null && serviceLinkZoneGroupedData.Any() ? serviceLinkZoneGroupedData.Count() : 0,
                SumCommercialUnit = serviceLinkZoneGroupedData?.Sum(i => i.CommercialUnit) ?? 0,
                SumDomesticUnit = serviceLinkZoneGroupedData?.Sum(i => i.DomesticUnit) ?? 0,
                SumOtherUnit = serviceLinkZoneGroupedData?.Sum(i => i.OtherUnit) ?? 0,
                TotalUnit = serviceLinkZoneGroupedData?.Sum(i => i.TotalUnit) ?? 0
            };

            var result = new ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>(ReportLiterals.ServiceLinkZoneGrouped, serviceLinkZoneGroupedHeader, serviceLinkZoneGroupedData);
            return result;
        }
    }
}
