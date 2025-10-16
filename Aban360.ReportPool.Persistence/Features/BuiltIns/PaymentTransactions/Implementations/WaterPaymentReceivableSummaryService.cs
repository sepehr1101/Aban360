using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
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
    internal sealed class WaterPaymentReceivableSummaryService : PaymentReceivableBase, IWaterPaymentReceivableSummaryService
    {
        public WaterPaymentReceivableSummaryService(IConfiguration configuration)
            : base(configuration)
        { 
        }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>> GetInfo(WaterPaymentReceivableInputDto input)
        {
            string reportTitle=ReportLiterals.WaterPaymentReceivableSummary + ReportLiterals.ByUsage;
            string groupingField = input.IsZone ? GroupingFields.ZoneTitle : GroupingFields.UsageTitle;
            string query = GetGroupedQuery(true, input.ZoneIds.HasValue(), groupingField);

            IEnumerable<WaterPaymentReceivableSummaryDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableSummaryDataOutputDto>(query, input);
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
                Title=reportTitle,
            };
            if (waterPaymentReceivableData is not null && waterPaymentReceivableData.Any())
            {
                waterPaymentReceivableHeader.RecordCount = waterPaymentReceivableData is not null && waterPaymentReceivableData.Any() ? waterPaymentReceivableData.Count() : 0;
                waterPaymentReceivableHeader.CustomerCount = waterPaymentReceivableData?.Sum(r => r.CustomerCount) ?? 0;
                waterPaymentReceivableHeader.BillCount = waterPaymentReceivableData?.Sum(r => r.BillCount) ?? 0;
                waterPaymentReceivableHeader.Amount = waterPaymentReceivableData?.Sum(r => r.Amount) ?? 0;
                waterPaymentReceivableHeader.DueCount = waterPaymentReceivableData?.Sum(r => r.DueCount) ?? 0;
                waterPaymentReceivableHeader.DueAmount = waterPaymentReceivableData?.Sum(r => r.DueAmount) ?? 0;
                waterPaymentReceivableHeader.OverdueCount = waterPaymentReceivableData?.Sum(r => r.OverdueCount) ?? 0;
                waterPaymentReceivableHeader.OverdueAmount = waterPaymentReceivableData?.Sum(r => r.OverdueAmount) ?? 0;
            }
            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>(reportTitle, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }
    }
}
