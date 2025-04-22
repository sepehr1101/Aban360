using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    [Route("v1/payment-procedure")]
    public class PaymentProcedureGetAllController : BaseController
    {
        private readonly IPaymentProcedureGetAllHandler _paymentProcedureGetAllHandler;
        public PaymentProcedureGetAllController(IPaymentProcedureGetAllHandler paymentProcedureGetAllHandler)
        {
            _paymentProcedureGetAllHandler = paymentProcedureGetAllHandler;
            _paymentProcedureGetAllHandler.NotNull(nameof(paymentProcedureGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<PaymentProcedureGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var paymentProcedures = await _paymentProcedureGetAllHandler.Handle(cancellationToken);
            return Ok(paymentProcedures);
        }
    }
}
