using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Commands;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Bill.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Commands
{
    [Route("v1/payment-id")]
    public class PaymentIdBatchUpdateController : BaseController
    {
        private readonly IPaymentIdsService _paymentIdBatchUpdateHandler;
        public PaymentIdBatchUpdateController(IPaymentIdsService paymentIdBatchUpdateHandler)
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
            int updatedCount = await _paymentIdBatchUpdateHandler.PaymentIdBatchUpdateAsync(inputDto, cancellationToken);
            return Ok(updatedCount);
        }
    }
}
