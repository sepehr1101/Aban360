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
    internal sealed class ServiceLinkPaymentDetailQueryService : PaymentBase, IServiceLinkPaymentDetailQueryService
    {
        public ServiceLinkPaymentDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>> GetInfo(PaymentDetailInputDto input)
        {
            string query = GetDetailQuery(false,input.ZoneIds?.Any()==true);
            //string query = GetServiceLinkPaymentDetailQuery(input.ZoneIds?.Any()==true);
            
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                fromBankId = input.FromBankId,
                toBankId = input.ToBankId,
                zoneIds =input.ZoneIds,
            };
            IEnumerable<PaymentDetailDataOutputDto> serviceLinkPaymentDetailData = await _sqlReportConnection.QueryAsync<PaymentDetailDataOutputDto>(query, @params);
            PaymentDetailHeaderOutputDto serviceLinkPaymentDetailHeader = new PaymentDetailHeaderOutputDto()
            {
                FromBankId = input.FromBankId,
                ToBankId = input.ToBankId,
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                ReportDateJalali=DateTime.Now.ToShortPersianDateString(),
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                CustomerCount = (serviceLinkPaymentDetailData is not null && serviceLinkPaymentDetailData.Any()) ? serviceLinkPaymentDetailData.Count() : 0,
                RecordCount = (serviceLinkPaymentDetailData is not null && serviceLinkPaymentDetailData.Any()) ? serviceLinkPaymentDetailData.Count() : 0,
                TotalAmount = serviceLinkPaymentDetailData.Sum(payment => Convert.ToUInt32(payment.Amount)),
            };
            var result = new ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>(ReportLiterals.ServiceLinkPaymentDetail, serviceLinkPaymentDetailHeader, serviceLinkPaymentDetailData);
            return result;
        }

        private string GetServiceLinkPaymentDetailQuery(bool hasZone)
        {
            string zoneQuery=hasZone? "AND p.ZoneId IN @ZoneIds":string.Empty;
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
                    From [CustomerWarehouse].dbo.PaymentsEn p
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
