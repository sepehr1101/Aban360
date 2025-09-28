using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class ServiceLinkPaymentReceivableSummaryService : AbstractBaseConnection, IServiceLinkPaymentReceivableSummaryService
    {
        public ServiceLinkPaymentReceivableSummaryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>> GetInfo(WaterPaymentReceivableInputDto input)
        {
            string paymentReceivables = GetWaterPaymentReceivableQuery(input.ZoneIds.Any(), input.IsZone);
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                fromBankId = input.FromBankId,
                toBankId = input.ToBankId,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<WaterPaymentReceivableSummaryDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableSummaryDataOutputDto>(paymentReceivables, @params);
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
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
            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>(ReportLiterals.WaterPaymentReceivableSummary + ReportLiterals.ByUsage, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }

        private string GetWaterPaymentReceivableQuery(bool hasZone, bool isZone)
        {
            string zoneQuery = hasZone ? "AND p.ZoneId IN @ZoneIds" : string.Empty;
            string param = isZone ? ReportLiterals.ZoneTitle : ReportLiterals.UsageTitle;
            return @$"";
        }
    }
}
