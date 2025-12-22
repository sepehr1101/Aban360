using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/service-link")]
    public class ServiceLinkCheckPayController : BaseController
    {
        private readonly IPaymentInquiryHandler _paymentInquiryHandler;
        private readonly IReportGenerator _reportGenerator;
        public ServiceLinkCheckPayController(
            IPaymentInquiryHandler paymentInquiryHandler,
            IReportGenerator reportGenerator)
        {
            _paymentInquiryHandler = paymentInquiryHandler;
            _paymentInquiryHandler.NotNull(nameof(paymentInquiryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("check_pay")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<PaymentInquiryOutputDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckPay(PaymentInquiryInputDto input, CancellationToken cancellationToken)
        {
            PaymentInquiryOutputDto PaymentInquiry = await _paymentInquiryHandler.Handle(input, cancellationToken);
            return Ok(PaymentInquiry);
        }
    }
}
