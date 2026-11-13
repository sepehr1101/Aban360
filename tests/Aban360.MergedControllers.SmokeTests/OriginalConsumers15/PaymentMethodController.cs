// Frozen consumer before service consolidation; test-only.
using BaseController = Aban360.Api.Controllers.V1.BaseController;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Application.Features.Remuneration.Handlers.Queries.Contracts;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.Api.Filters;

namespace BeforeServiceConsumers15.Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    public class PaymentMethodController : BaseController
    {
        private readonly IPaymentMethodGetAllHandler _c0_paymentMethodGetAllHandler;
        private readonly IPaymentMethodGetSingleHandler _c1_paymentMethodGetSingleHandler;
        public PaymentMethodController(IPaymentMethodGetAllHandler c0_paymentMethodGetAllHandler, IPaymentMethodGetSingleHandler c1_paymentMethodGetSingleHandler)
        {
            _c0_paymentMethodGetAllHandler = c0_paymentMethodGetAllHandler;
            _c0_paymentMethodGetAllHandler.NotNull(nameof(c0_paymentMethodGetAllHandler));
            _c1_paymentMethodGetSingleHandler = c1_paymentMethodGetSingleHandler;
            _c1_paymentMethodGetSingleHandler.NotNull(nameof(c1_paymentMethodGetSingleHandler));
        }

        [HttpPost, HttpGet]
        [Route("~/v1/payment-method/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<PaymentMethodGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var paymentMethods = await _c0_paymentMethodGetAllHandler.Handle(cancellationToken);
            return Ok(paymentMethods);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/payment-method/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<PaymentMethodGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(PaymentMethodEnum id, CancellationToken cancellationToken)
        {
            var paymentMethods = await _c1_paymentMethodGetSingleHandler.Handle(id, cancellationToken);
            return Ok(paymentMethods);
        }
    }
}
