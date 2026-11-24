using Aban360.CalculationPool.Application.Features.Bill.Handlers.Queries.Contracts;
using Aban360.CalculationPool.Domain.Features.Bill.Dtos.Queries;
using Aban360.Common.BaseEntities;
using Aban360.Common.Categories.ApiResponse;
using Aban360.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Aban360.CalculationPool.Application.Features.Bill.Services;

namespace Aban360.Api.Controllers.V1.CalculationPool.Bill.Queries
{
    [Route("v1/bill")]
    public class InvalidPaymentIdController : BaseController
    {
        private readonly IPaymentIdsService _invalidPaymentIdGetHandler;
        public InvalidPaymentIdController(IPaymentIdsService invalidPaymentIdGetHandler)
        {
            _invalidPaymentIdGetHandler = invalidPaymentIdGetHandler;
            _invalidPaymentIdGetHandler.NotNull(nameof(invalidPaymentIdGetHandler));
        }

        [HttpPost]
        [Route("invalid-payment-id")]
        [ProducesResponseType(typeof(ApiResponseEnvelope<ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromBody] InvalidPaymentIdInputDto inputDto, CancellationToken cancellationToken)
        {
            ReportOutput<InvalidPaymentIdHeaderOutputDto, InvalidPaymentIdDataOutputDto> companyServiceTypes = await _invalidPaymentIdGetHandler.InvalidPaymentIdGetAsync(inputDto, CurrentUser, cancellationToken);
            return Ok(companyServiceTypes);
        }
    }
}
