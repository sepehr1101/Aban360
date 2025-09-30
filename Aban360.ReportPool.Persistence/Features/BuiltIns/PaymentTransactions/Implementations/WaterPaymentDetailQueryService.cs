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
    internal sealed class WaterPaymentDetailQueryService : AbstractBaseConnection, IWaterPaymentDetailQueryService
    {
        public WaterPaymentDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>> GetInfo(PaymentDetailInputDto input)
        {
            string waterPaymentDetails = GetWaterPaymentDetailQuery(input.ZoneIds?.Any()==true);
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                fromBankId=input.FromBankId,
                toBankId=input.ToBankId,
                zoneIds=input.ZoneIds,
            };
            IEnumerable<PaymentDetailDataOutputDto> waterPaymentDetailData = await _sqlReportConnection.QueryAsync<PaymentDetailDataOutputDto>(waterPaymentDetails, @params);
            PaymentDetailHeaderOutputDto waterPaymentDetailHeader = new PaymentDetailHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                ReportDateJalali=DateTime.Now.ToShortPersianDateString(),
                RecordCount = (waterPaymentDetailData is not null && waterPaymentDetailData.Any()) ? waterPaymentDetailData.Count() : 0,
                CustomerCount = (waterPaymentDetailData is not null && waterPaymentDetailData.Any()) ? waterPaymentDetailData.Count() : 0,
                TotalAmount = waterPaymentDetailData.Sum(payment => Convert.ToInt64(payment.Amount)),
            };

            var result = new ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>(ReportLiterals.WaterPaymentDetail, waterPaymentDetailHeader, waterPaymentDetailData);
            return result;
        }

        private string GetWaterPaymentDetailQuery(bool hasZone)
        {
            string zoneQuery = hasZone ? "AND p.ZoneId IN @ZoneIds" : string.Empty;
            return @$"Select
                     	p.CustomerNumber As CustomerNumber,
                    	p.PayDateJalali AS BankDateJalali,
                    	p.BankCode AS BankCode,
                    	p.RegisterDay AS EventBankDateJalali,
                    	p.BillId AS BillId,
                    	p.PaymentGateway AS PaymentMethodTitle,
                    	p.RegisterDay AS PaymentDate,
                    	p.Amount AS Amount,
                        p.BankName AS BankName
                    From [CustomerWarehouse].dbo.Payments p
                    WHERE 
                    	(@FromDate IS  NULL 
                    		OR @ToDate IS NULL 
                    		OR p.RegisterDay BETWEEN @FromDate AND @ToDate) 
                    	AND(@FromAmount IS  NULL 
                    		OR @ToAmount IS NULL 
                    		OR p.Amount BETWEEN @FromAmount AND @ToAmount)
                        AND(@fromBankId IS NULL OR
						    @toBankId IS NULL OR
						    p.BankCode BETWEEN @fromBankId AND @toBankId)
                        {zoneQuery}";
        }
    }
}
