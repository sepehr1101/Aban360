using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/payment")]
    public class GeneratePaymentIdController : BaseController
    {
        private readonly IGeneratePaymentIdGetHandler _paymentIdHandler;
        public GeneratePaymentIdController(IGeneratePaymentIdGetHandler paymentIdHandler)
        {
            _paymentIdHandler = paymentIdHandler;
            _paymentIdHandler.NotNull(nameof(paymentIdHandler));
        }

        [HttpPost, HttpGet]
        [Route("generate-payment-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GeneratePaymentId(GeneratePaymentIdInputDto inputDto, CancellationToken cancellationToken)
        {
            string paymentId = await _paymentIdHandler.Handle(inputDto, CurrentUser, cancellationToken);
            return Ok(paymentId);
        }
    }
}
