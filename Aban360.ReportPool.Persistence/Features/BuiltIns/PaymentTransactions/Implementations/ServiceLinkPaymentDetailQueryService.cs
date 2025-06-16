using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Aban360.ReportPool.Persistence.Base;
using Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Contracts;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Aban360.ReportPool.Persistence.Features.BuiltIns.PaymentTransactions.Implementations
{
    internal sealed class ServiceLinkPaymentDetailQueryService : AbstractBaseConnection, IServiceLinkPaymentDetailQueryService
    {
        public ServiceLinkPaymentDetailQueryService(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>> GetInfo(PaymentDetailInputDto input)
        {
            string serviceLinkPaymentDetails = GetServiceLinkPaymentDetailQuery();
            var @params = new
            {
                FromDate = input.FromDateJalali,
                ToDate = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
            };
            IEnumerable<PaymentDetailDataOutputDto> serviceLinkPaymentDetailData = await _sqlReportConnection.QueryAsync<PaymentDetailDataOutputDto>(serviceLinkPaymentDetails, @params);
            PaymentDetailHeaderOutputDto serviceLinkPaymentDetailHeader = new PaymentDetailHeaderOutputDto()
            {
                FromDateJalali = input.FromDateJalali,
                ToDateJalali = input.ToDateJalali,
                FromAmount = input.FromAmount,
                ToAmount = input.ToAmount,
                RecordCount = serviceLinkPaymentDetailData.Count(),
                TotalRegisterAmount = serviceLinkPaymentDetailData.Sum(payment => Convert.ToUInt32(payment.RegisterAmount)),
            };
            var result = new ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>(ReportLiterals.ServiceLinkPaymentDetail, serviceLinkPaymentDetailHeader, serviceLinkPaymentDetailData);
            return result;
        }

        private string GetServiceLinkPaymentDetailQuery()
        {
            //ToDo: choise ServiceLink Payment
            return @"Select
                     	p.CustomerNumber As CustomerNumber,
                    	p.RegisterDay AS BankDateJalali,
                    	p.BankBranchCode AS BankCode,--BankCode?
                    	p.RegisterDay AS EventBankDateJalali,
                    	p.BankBranchCode AS SerialNumber,--SerialBank?
                    	p.BillId AS BillId,
                    	p.PaymentGateway AS PaymentMethodTitle,
                    	p.RegisterDay AS PaymentDate,--PaymentDate
                    	p.Amount AS RegisterAmount,
                        p.BankName AS BankName
                    From [CustomerWarehouse].dbo.Payments p
                    WHERE 
                    	(@FromDate IS  NULL 
                    		OR @ToDate IS NULL 
                    		OR p.RegisterDay BETWEEN @FromDate AND @ToDate) 
                    	AND(@FromAmount IS  NULL 
                    		OR @ToAmount IS NULL 
                    		OR p.Amount BETWEEN @FromAmount AND @ToAmount)";
        }
    }
}
