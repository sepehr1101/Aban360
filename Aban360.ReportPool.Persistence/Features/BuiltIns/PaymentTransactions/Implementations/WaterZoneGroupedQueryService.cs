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
    internal sealed class WaterZoneGroupedQueryService : PaymentBase, IWaterZoneGroupedQueryService
    {
        public WaterZoneGroupedQueryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>> GetInfo(ServiceLinkWaterItemGroupedInputDto input)
        {
            string query = GetGroupedQuery(true, input.ZoneIds.HasValue(), GroupingFields.ZoneTitle);
          
            IEnumerable<ServiceLinkWaterItemGroupedDataOutputDto> waterZoneGroupedData = await _sqlReportConnection.QueryAsync<ServiceLinkWaterItemGroupedDataOutputDto>(query, input);
            ServiceLinkWaterItemGroupedHeaderOutputDto waterZoneGroupedHeader = new ServiceLinkWaterItemGroupedHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromBankId = input.FromBankId,
                ToBankId = input.ToBankId,
                RecordCount = waterZoneGroupedData is not null && waterZoneGroupedData.Any() ? waterZoneGroupedData.Count() : 0,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                TotalAmount = waterZoneGroupedData.Sum(zone => zone.Amount),
                Title = ReportLiterals.WaterZoneGrouped,

                CustomerCount = waterZoneGroupedData is not null && waterZoneGroupedData.Any() ? waterZoneGroupedData.Count() : 0,
                SumCommercialUnit = waterZoneGroupedData?.Sum(i => i.CommercialUnit) ?? 0,
                SumDomesticUnit = waterZoneGroupedData?.Sum(i => i.DomesticUnit) ?? 0,
                SumOtherUnit = waterZoneGroupedData?.Sum(i => i.OtherUnit) ?? 0,
                TotalUnit = waterZoneGroupedData?.Sum(i => i.TotalUnit) ?? 0
            };

            var result = new ReportOutput<ServiceLinkWaterItemGroupedHeaderOutputDto, ServiceLinkWaterItemGroupedDataOutputDto>(ReportLiterals.WaterZoneGrouped, waterZoneGroupedHeader, waterZoneGroupedData);
            return result;
        }
    }
}
