using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    [Route("v1/payment-method")]
    public class PaymentMethodGetSingleController : BaseController
    {
        private readonly IPaymentMethodGetSingleHandler _paymentMethodGetSingleHandler;
        public PaymentMethodGetSingleController(IPaymentMethodGetSingleHandler paymentMethodGetSingleHandler)
        {
            _paymentMethodGetSingleHandler = paymentMethodGetSingleHandler;
            _paymentMethodGetSingleHandler.NotNull(nameof(paymentMethodGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<PaymentMethodGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(PaymentMethodEnum id, CancellationToken cancellationToken)
        {
            var paymentMethods = await _paymentMethodGetSingleHandler.Handle(id, cancellationToken);
            return Ok(paymentMethods);
        }
    }
}
