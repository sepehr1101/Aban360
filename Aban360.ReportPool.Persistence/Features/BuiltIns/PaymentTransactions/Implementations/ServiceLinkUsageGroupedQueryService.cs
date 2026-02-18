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
    internal sealed class ServiceLinkUsageGroupedQueryService : PaymentBase, IServiceLinkUsageGroupedQueryService
    {
        public ServiceLinkUsageGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>> GetInfo(ServiceLinkWaterItemGroupedInputDto input)
        {
            string query = GetGroupedQuery(false, input.ZoneIds.HasValue(), GroupingFields.UsageTitle);
            
            IEnumerable<ServiceLinkWaterItemGroupedDataOutputDto> serviceLinkUsageGroupedData = await _sqlReportConnection.QueryAsync<ServiceLinkWaterItemGroupedDataOutputDto>(query, input);
            ServiceLinkWaterItemGroupedHeaderOutputDto serviceLinkUsageGroupedHeader = new ServiceLinkWaterItemGroupedHeaderOutputDto()
            {
                FromBankId = input.FromBankId,
                ToBankId = input.ToBankId,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                RecordCount = serviceLinkUsageGroupedData is not null && serviceLinkUsageGroupedData.Any() ? serviceLinkUsageGroupedData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                TotalAmount = serviceLinkUsageGroupedData.Sum(usage => usage.Amount),
                Title= ReportLiterals.ServiceLinkUsageGrouped,

                CustomerCount = serviceLinkUsageGroupedData?.Sum(i=>i.CustomerCount)??0,
                SumCommercialUnit = serviceLinkUsageGroupedData?.Sum(i => i.CommercialUnit) ?? 0,
                SumDomesticUnit = serviceLinkUsageGroupedData?.Sum(i => i.DomesticUnit) ?? 0,
                SumOtherUnit = serviceLinkUsageGroupedData?.Sum(i => i.OtherUnit) ?? 0,
                TotalUnit = serviceLinkUsageGroupedData?.Sum(i => i.TotalUnit) ?? 0
            };

            var result = new ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>(ReportLiterals.ServiceLinkUsageGrouped, serviceLinkUsageGroupedHeader, serviceLinkUsageGroupedData);
            return result;
        }
    }
}
