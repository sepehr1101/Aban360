using Aban360.Api.Cronjobs;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/unspecified-service-link-payment")]
    public class UnspecifiedServiceLinkPaymentController : BaseController
    {
        private readonly IUnspecifiedServiceLinkPaymentHandler _unspecifiedServiceLinkPayment;
        private readonly IReportGenerator _reportGenerator;
        public UnspecifiedServiceLinkPaymentController(
            IUnspecifiedServiceLinkPaymentHandler unspecifiedServiceLinkPayment,
            IReportGenerator reportGenerator)
        {
            _unspecifiedServiceLinkPayment = unspecifiedServiceLinkPayment;
            _unspecifiedServiceLinkPayment.NotNull(nameof(_unspecifiedServiceLinkPayment));

            _reportGenerator = reportGenerator;
            _reportGenerator.NotNull(nameof(_reportGenerator));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(UnspecifiedPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<UnspecifiedPaymentHeaderOutputDto, UnspecifiedPaymentDataOutputDto> unspecifiedServiceLink = await _unspecifiedServiceLinkPayment.Handle(inputDto, cancellationToken);
            return Ok(unspecifiedServiceLink);
        }

        [HttpPost, HttpGet]
        [Route("excel/{connectionId}")]
        public async Task<IActionResult> GetExcel(string connectionId, UnspecifiedPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            await _reportGenerator.FireAndInform(inputDto, cancellationToken, _unspecifiedServiceLinkPayment.Handle, CurrentUser, ReportLiterals.UnspecifiedServiceLinkPayment, connectionId);
            return Ok(inputDto);
        }
    }
}
