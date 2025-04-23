using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    [Route("v1/payment_method")]
    public class PaymentMethodGetAllController : BaseController
    {
        private readonly IPaymentMethodGetAllHandler _paymentMethodGetAllHandler;
        public PaymentMethodGetAllController(IPaymentMethodGetAllHandler paymentMethodGetAllHandler)
        {
            _paymentMethodGetAllHandler = paymentMethodGetAllHandler;
            _paymentMethodGetAllHandler.NotNull(nameof(paymentMethodGetAllHandler));
        }

        [HttpPost, HttpGet]
        [Route("all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<PaymentMethodGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var paymentMethods = await _paymentMethodGetAllHandler.Handle(cancellationToken);
            return Ok(paymentMethods);
        }
    }
}
