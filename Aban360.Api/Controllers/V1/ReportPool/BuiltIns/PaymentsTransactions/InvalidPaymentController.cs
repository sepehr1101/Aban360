using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.ReportPool.Application.Features.BuiltsIns.PaymentTransacionts.Handlers.Contracts;
using Aban360.ReportPool.Domain.Base;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Inputs;
using Aban360.ReportPool.Domain.Features.BuiltIns.PaymentsTransactions.Outputs;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.ReportPool.BuiltIns.PaymentsTransactions
{
    [Route("v1/invalid-payment")]
    public class InvalidPaymentController : BaseController
    {
        private readonly IInvalidPaymentHandler _invalidPayment;
        public InvalidPaymentController(IInvalidPaymentHandler invalidPayment)
        {
            _invalidPayment = invalidPayment;
            _invalidPayment.NotNull(nameof(_invalidPayment));
        }

        [HttpPost, HttpGet]
        [Route("raw")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRaw(InvalidPaymentInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InvalidPaymentHeaderOutputDto, InvalidPaymentDataOutputDto> invalidPayment = await _invalidPayment.Handle(inputDto, cancellationToken);
            return Ok(invalidPayment);
        }
    }
}
