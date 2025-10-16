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
    internal sealed class ServiceLinkPaymentReceivableSummaryService : PaymentReceivableBase, IServiceLinkPaymentReceivableSummaryService
    {
        public ServiceLinkPaymentReceivableSummaryService(IConfiguration configuration)
            : base(configuration)
        {
        }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>> GetInfo(ServiceLinkPaymentReceivableInputDto input)
        {
            string reportTitle = ReportLiterals.ServiceLinkPaymentReceivableSummary + ReportLiterals.ByUsage;
            string groupingField = input.IsZone ? GroupingFields.ZoneTitle : GroupingFields.UsageTitle;
            string query = GetGroupedQuery(false, input.ZoneIds.HasValue(), groupingField, input.UsageIds.HasValue());

            IEnumerable<WaterPaymentReceivableSummaryDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableSummaryDataOutputDto>(query, input);
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                RecordCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0,
                Title=reportTitle,
            };

            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>(reportTitle, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }
    }
}
