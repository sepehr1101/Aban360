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
            IEnumerable<PaymentDetailDataOutputDto> serviceLinkPaymentDetailDate = await _sqlConnection.QueryAsync<PaymentDetailDataOutputDto>(serviceLinkPaymentDetails);//todo: Parameters
            PaymentDetailHeaderOutputDto serviceLinkPaymentDetailHeader = new PaymentDetailHeaderOutputDto()
            { };

            var result = new ReportOutput<PaymentDetailHeaderOutputDto, PaymentDetailDataOutputDto>(ReportLiterals.ServiceLinkPaymentDetail, serviceLinkPaymentDetailHeader, serviceLinkPaymentDetailDate);
            return result;
        }

        private string GetServiceLinkPaymentDetailQuery()
        {
            return @" ";
        }
    }
}
