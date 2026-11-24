using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Bill.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/payment")]
    public class GeneratePaymentIdController : BaseController
    {
        private readonly IPaymentIdsService _paymentIdHandler;
        public GeneratePaymentIdController(IPaymentIdsService paymentIdHandler)
        {
            _paymentIdHandler = paymentIdHandler;
            _paymentIdHandler.NotNull(nameof(paymentIdHandler));
        }

        [HttpPost]
        [Route("generate-payment-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GeneratePaymentId(GeneratePaymentIdInputDto inputDto, CancellationToken cancellationToken)
        {
            string paymentId = await _paymentIdHandler.GeneratePaymentIdGetAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(paymentId);
        }
    }
}
