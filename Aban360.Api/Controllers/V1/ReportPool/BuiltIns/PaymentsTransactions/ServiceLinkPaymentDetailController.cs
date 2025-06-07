using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/service-link-payment-detail")]
    public class ServiceLinkPaymentDetailController : BaseController
    {
        private readonly IServiceLinkPaymentDetailHandler _serviceLinkPaymentDetail;
        public ServiceLinkPaymentDetailController(IServiceLinkPaymentDetailHandler serviceLinkPaymentDetail)
        {
            _serviceLinkPaymentDetail = serviceLinkPaymentDetail;
            _serviceLinkPaymentDetail.NotNull(nameof(_serviceLinkPaymentDetail));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<ServiceLinkPaymentDetailHeaderOutputDto, ServiceLinkPaymentDetailDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(ServiceLinkPaymentDetailInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<ServiceLinkPaymentDetailHeaderOutputDto, ServiceLinkPaymentDetailDataOutputDto> serviceLinkPaymentDetail = await _serviceLinkPaymentDetail.Handle(inputDto, cancellationToken);
            return Ok(serviceLinkPaymentDetail);
        }
    }
}
