using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    [Route("v1/payment-procedure")]
    public class PaymentProcedureGetSingleController : BaseController
    {
        private readonly IPaymentProcedureGetSingleHandler _paymentProcedureGetSingleHandler;
        public PaymentProcedureGetSingleController(IPaymentProcedureGetSingleHandler paymentProcedureGetSingleHandler)
        {
            _paymentProcedureGetSingleHandler = paymentProcedureGetSingleHandler;
            _paymentProcedureGetSingleHandler.NotNull(nameof(paymentProcedureGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<PaymentProcedureGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(PaymentProcedureEnum id, CancellationToken cancellationToken)
        {
            var paymentProcedures = await _paymentProcedureGetSingleHandler.Handle(id, cancellationToken);
            return Ok(paymentProcedures);
        }
    }
}
