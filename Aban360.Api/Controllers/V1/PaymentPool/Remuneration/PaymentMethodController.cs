using Aban360.PaymentPool.Application.Features.Remuneration.Services;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Aban360.PaymentPool.Domain.Features.Remuneration.Dto.Queries;
using Microsoft.AspNetCore.Mvc;
using Aban360.PaymentPool.Domain.Constansts;
using Aban360.Api.Filters;

namespace Aban360.Api.Controllers.V1.PaymentPool.Remuneration
{
    public class PaymentMethodController : BaseController
    {
        private readonly IPaymentMethodService _service;

        public PaymentMethodController(IPaymentMethodService service)
        {
            service.NotNull(nameof(service));
            _service = service;
        }

        [HttpPost, HttpGet]
        [Route("~/v1/payment-method/all")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ICollection<PaymentMethodGetDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var paymentMethods = await _service.GetAllAsync(cancellationToken);
            return Ok(paymentMethods);
        }

        [HttpPost, HttpGet]
        [Route("~/v1/payment-method/single/{id}")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<PaymentMethodGetDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSingle(PaymentMethodEnum id, CancellationToken cancellationToken)
        {
            var paymentMethods = await _service.GetSingleAsync(id, cancellationToken);
            return Ok(paymentMethods);
        }
    }
}
