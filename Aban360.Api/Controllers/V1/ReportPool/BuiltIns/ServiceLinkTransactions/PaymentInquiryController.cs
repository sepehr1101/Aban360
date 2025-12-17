using Aban360.Api.Cronjobs;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.ServiceLinkTransactions.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.ServiceLinkTransaction.Outputs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.ServiceLinkTransactions
{
    [Route("v1/payment-inquiry")]
    public class PaymentInquiryController : BaseController
    {
        private readonly IPaymentInquiryHandler _paymentInquiryHandler;
        private readonly IReportGenerator _reportGenerator;
        public PaymentInquiryController(
            IPaymentInquiryHandler paymentInquiryHandler,
            IReportGenerator reportGenerator)
        {
            _paymentInquiryHandler = paymentInquiryHandler;
            _paymentInquiryHandler.NotNull(nameof(paymentInquiryHandler));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<PaymentInquiryHeaderOutputDto, PaymentInquiryDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(PaymentInquiryInputDto input, CancellationToken cancellationToken)
        {
            ReportOutput<PaymentInquiryHeaderOutputDto, PaymentInquiryDataOutputDto> PaymentInquiry = await _paymentInquiryHandler.Handle(input, cancellationToken);
            return Ok(PaymentInquiry);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, PaymentInquiryInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _paymentInquiryHandler.Handle, CurrentUser, ReportLiterals.PaymentInquiry, connectionId);
            return Ok(inputDto);
        }

        [HttpPost]
        [Route("sti")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<JsonReportId>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetStiReport(PaymentInquiryInputDto inputDto, CancellationToken cancellationToken)
        {
            int reportCode = 720;
            ReportOutput<PaymentInquiryHeaderOutputDto, PaymentInquiryDataOutputDto> result = await _paymentInquiryHandler.Handle(inputDto, cancellationToken);
            JsonReportId reportId = await JsonOperation.ExportToJson(result, cancellationToken, reportCode);
            return Ok(reportId);
        }
    }
}
