using Aban360.Common.BaseEntities;
using Aban360.Common.Db.Dapper;
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
        { }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableSummaryDataOutputDto>> GetInfo(WaterPaymentReceivableInputDto input)
        {
            string groupingField = input.IsZone ? GroupingFields.ZoneTitle : GroupingFields.UsageTitle;
            string query = GetGroupedQuery(true, input.ZoneIds?.Any() == true, groupingField);
            //string query = GetWaterPaymentReceivableQuery(input.ZoneIds?.Any() == true, input.IsZone);

            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                fromBankId = input.FromBankId,
                toBankId = input.ToBankId,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<WaterPaymentReceivableSummaryDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableSummaryDataOutputDto>(query, @params);
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
            return @$"Select 
                        b.{param} as ItemTitle,
                    	COUNT(1) as BillCount,
						COUNT(Distinct b.BillId) as CustomerCount,
						SUM(p.Amount) as Amount,
						SUM(Case When p.PayDateJalali<=b.DeadLine Then p.Amount Else 0 End) as DueAmount,
						SUM(Case When p.PayDateJalali>b.DeadLine Then p.Amount Else 0 End ) as OverdueAmount,
						Count(Case When p.PayDateJalali<=b.DeadLine Then 1 End) as DueCount,
						Count(Case When p.PayDateJalali>b.DeadLine Then 1 End ) as OverdueCount
                    From [CustomerWarehouse].dbo.Bills b
                    LEFT JOIN [CustomerWarehouse].dbo.Payments p ON p.BillTableId=b.Id
                    WHERE
                        (@FromDate IS NULL
                     	    OR @ToDate IS NULL 
                     	    OR p.RegisterDay BETWEEN @FromDate AND @ToDate)
                        AND (@fromBankId IS NULL OR
						    @toBankId IS NULL OR
						    p.BankCode BETWEEN @fromBankId AND @toBankId)
                        {zoneQuery}
						Group By b.{param}";
        }
    }
}
