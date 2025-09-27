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
    internal sealed class WaterPaymentReceivableQueryService : AbstractBaseConnection, IWaterPaymentReceivableQueryService
    {
        public WaterPaymentReceivableQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>> GetInfo(WaterPaymentReceivableInputDto input)
        {
            string paymentReceivables = GetWaterPaymentReceivableQuery();
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
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
                waterPaymentReceivableHeader.SumTotalCount = waterPaymentReceivableData.Sum(payment => payment.TotalCount);
                waterPaymentReceivableHeader.SumTotalAmount = waterPaymentReceivableData.Sum(payment => payment.TotalAmount);
                waterPaymentReceivableHeader.SumOverdueCount = waterPaymentReceivableData.Sum(payment => payment.OverdueCount);
                waterPaymentReceivableHeader.SumOverdueAmount = waterPaymentReceivableData.Sum(payment => payment.OverdueAmount);
                waterPaymentReceivableHeader.SumCurrentAmount = waterPaymentReceivableData.Sum(payment => payment.CurrentAmount);
                waterPaymentReceivableHeader.CustomerCount = waterPaymentReceivableData.Sum(payment => payment.TotalCount);
            }
            var result = new ReportOutput<WaterPaymentReceivableHeaderOutputDto, WaterPaymentReceivableDataOutputDto>(ReportLiterals.WaterPaymentReceivable, waterPaymentReceivableHeader, waterPaymentReceivableData);
            return result;
        }

        private string GetWaterPaymentReceivableQuery()
        {
            return @"Select 
                    	Max(b.BillId),
                    	b.UsageTitle AS UsageTitle,
						SUM(DISTINCT b.Payable) AS TotalAmount,
                        COUNT(DISTINCT b.BillId) - COUNT(DISTINCT p.BillId) AS OverdueCount ,
                        SUM(DISTINCT b.Payable) - SUM(DISTINCT p.Amount) AS OverdueAmount ,
						SUM(DISTINCT p.Amount) AS CurrentAmount,
						COUNT(DISTINCT b.BillId) AS TotalCount
                    From [CustomerWarehouse].dbo.Bills b
                    LEFT JOIN [CustomerWarehouse].dbo.Payments p ON p.BillTableId=b.Id
                    WHERE
                    	@FromDate IS NULL
                    	OR @ToDate IS NULL 
                    	OR p.RegisterDay BETWEEN @FromDate AND @ToDate
                    GROUP BY b.UsageTitle";
        }
    }
}
