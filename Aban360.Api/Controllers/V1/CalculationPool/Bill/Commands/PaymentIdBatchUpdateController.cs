using Aban360.CalculationPool.Application.Features.Bill.Handlers.Commands.Update.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/payment-id")]
    public class PaymentIdBatchUpdateController : BaseController
    {
        private readonly IPaymentIdBatchUpdateHandler _paymentIdBatchUpdateHandler;

        public PaymentIdBatchUpdateController(IPaymentIdBatchUpdateHandler paymentIdBatchUpdateHandler)
        {
            _paymentIdBatchUpdateHandler = paymentIdBatchUpdateHandler;
            _paymentIdBatchUpdateHandler.NotNull(nameof(paymentIdBatchUpdateHandler));
        }

        [HttpPost]
        [Route("batch-update")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<int>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] PaymentIdBatchUpdateInputDto inputDto, CancellationToken cancellationToken)
        {
            int updatedCount = await _paymentIdBatchUpdateHandler.Handle(inputDto, cancellationToken);
            return Ok(updatedCount);
        }
    }
}
