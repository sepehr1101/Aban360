using Aban360.Common.BaseEntities;
using Aban360.Common.Extensions;
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
        { 
        }

        public async Task<ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>> GetInfo(PaymentDetailInputDto input)
        {
            string query = GetDetailQuery(false,input.ZoneIds.HasValue());

            IEnumerable<PaymentDetailDataOutputDto> serviceLinkPaymentDetailData = await _sqlReportConnection.QueryAsync<PaymentDetailDataOutputDto>(query, input);
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
                Title= ReportLiterals.ServiceLinkPaymentDetail,
            };
            var result = new ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>(ReportLiterals.ServiceLinkPaymentDetail, serviceLinkPaymentDetailHeader, serviceLinkPaymentDetailData);
            return result;
        }
    }
}
