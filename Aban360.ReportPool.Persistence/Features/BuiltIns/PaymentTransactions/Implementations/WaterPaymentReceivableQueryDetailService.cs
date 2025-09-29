using Aban360.Common.BaseEntities;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using DNTPersianUtils.Core;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class WaterPaymentReceivableQueryDetailService : AbstractBaseConnection, IWaterPaymentReceivableQueryDetailService
    {
        public WaterPaymentReceivableQueryDetailService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>> GetInfo(WaterPaymentReceivableInputDto input)
        {
            string paymentReceivables = GetWaterPaymentReceivableQuery(input.ZoneIds.Any());
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                fromBankId = input.FromBankId,
                toBankId = input.ToBankId,
                zoneIds = input.ZoneIds,
            };
            IEnumerable<WaterPaymentReceivableDataOutputDto> waterPaymentReceivableData = await _sqlReportConnection.QueryAsync<WaterPaymentReceivableDataOutputDto>(paymentReceivables, @params);
            WaterPaymentReceivableHeaderOutputDto waterPaymentReceivableHeader = new WaterPaymentReceivableHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali = DateTime.Now.ToShortPersianDateString(),
            };
            if (waterPaymentReceivableData is not null && waterPaymentReceivableData.Any())
            {
                waterPaymentReceivableHeader.RecordCount = (waterPaymentReceivableData is not null && waterPaymentReceivableData.Any()) ? waterPaymentReceivableData.Count() : 0;  
                waterPaymentReceivableHeader.CustomerCount = (waterPaymentReceivableData is not null && waterPaymentReceivableData.Any()) ? waterPaymentReceivableData.Count() : 0;  
                waterPaymentReceivableHeader.Amount = waterPaymentReceivableData?.Sum(r=>r.Amount) ?? 0;                
            }
            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>(ReportLiterals.WaterPaymentReceivableDetail, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }

        private string GetWaterPaymentReceivableQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND p.ZoneId IN @ZoneIds" : string.Empty;
            return @$"Select 
                    	b.BillId,
						p.PayId,
						b.ZoneTitle,
                    	b.UsageTitle ,
						p.Amount,
						b.RegisterDay as BillIssueDateJalali,
						b.Deadline,
						p.PayDateJalali,
						IIF(p.PayDateJalali<=b.DeadLine,N'{ReportLiterals.Due}' , N'{ReportLiterals.Overdue}') AS AmountState
                    From [CustomerWarehouse].dbo.Bills b
                    LEFT JOIN [CustomerWarehouse].dbo.Payments p ON p.BillTableId=b.Id
                    WHERE
                        (@FromDate IS NULL
                     	    OR @ToDate IS NULL 
                     	    OR p.RegisterDay BETWEEN @FromDate AND @ToDate)
                        AND (@fromBankId IS NULL OR
						    @toBankId IS NULL OR
						    p.BankCode BETWEEN @fromBankId AND @toBankId)
                         {zoneQuery}";
        }
    }
}
